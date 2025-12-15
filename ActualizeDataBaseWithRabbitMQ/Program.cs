using ActualizeDataBaseWithRabbitMQ.Domain.Entities;
using ActualizeDataBaseWithRabbitMQ.Infrastructure;
using ActualizeDataBaseWithRabbitMQ.Infrastructure.LogInCrud;
using ActualizeDataBaseWithRabbitMQ.Infrastructure.PurchaseStocks;
using ActualizeDataBaseWithRabbitMQ.Infrastructure.SellStocksCruds;
using ActualizeDataBaseWithRabbitMQ.Infrastructure.StocksAppCruds;
using Hangfire;
using Hangfire.PostgreSql;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RabbitMQAndGenericRepository.RabbitMq;

var builder = WebApplication.CreateBuilder(args);
builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenLocalhost(5004); // Fuerza que siempre use el puerto 5004
});
var services = builder.Services;
var configuration = builder.Configuration;

// --------------------------------------------
//  PostgreSQL: registrar los 3 contextos
// --------------------------------------------
services.AddDbContext<SellStocksDbContext>(options =>
    options.UseNpgsql(configuration.GetConnectionString("SellStocksDb")));

services.AddDbContext<PurchaseStocksDbContext>(options =>
    options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

services.AddDbContext<StocksAppDbContext>(options =>
    options.UseNpgsql(configuration.GetConnectionString("StocksApp")));
services.AddDbContext<LogInDbContext>(options =>
    options.UseNpgsql(configuration.GetConnectionString("LogInDb")));

// --------------------------------------------
// 🔹 RabbitMQ
// --------------------------------------------
var envString = configuration.GetConnectionString("ConnectionString_Rabbit");
var rabbitMQOptions = RabbitMQOptions.ParseRabbitOptions(envString);
services.AddSingleton(rabbitMQOptions);
services.AddSingleton<RabbitMessageService>();

// --------------------------------------------
//  Repositorios y Jobs
// --------------------------------------------
services.AddScoped<StocksAppCrudStocks>();
services.AddScoped<StocksAppCrudUsers>();
services.AddScoped<StocksAppCrudPossession>();
services.AddScoped<StocksAppCrudPrices>();

services.AddScoped<SellStocksCrudStocks>();
services.AddScoped<SellStocksCrudPrices>();
services.AddScoped<SellStocksCrudUsers>();
services.AddScoped<SellStocksCrudPossession>();

services.AddScoped<PurchaseStocksCrudStocks>();
services.AddScoped<PurchaseStocksCrudUsers>();
services.AddScoped<PurchaseStocksCrudPossession>();
services.AddScoped<PurchaseStocksCrudPrices>();

services.AddScoped<LogInCrudUsers>();

services.AddScoped<AddToDbJob>();
services.AddScoped<DeleteFromDbJob>();
services.AddScoped<DbJob>();

// --------------------------------------------
//  Hangfire (solo una instancia, la principal)
// --------------------------------------------
var hangfireConn = configuration.GetConnectionString("SellStocksDb");
services.AddHangfire(config => config.UsePostgreSqlStorage(hangfireConn));
services.AddHangfireServer();

var app = builder.Build();

// --------------------------------------------
//  Dashboard de Hangfire
// --------------------------------------------
app.UseHangfireDashboard("/hangfire");

// --------------------------------------------
// Registro del Job
// --------------------------------------------
RecurringJob.AddOrUpdate<DbJob>(
    "job-sincronizacion",
    job => job.Execute(),
    Cron.Minutely
);

// --------------------------------------------
//  Endpoint de prueba
// --------------------------------------------
app.MapGet("/", () => "Servidor corriendo correctamente con múltiples bases y Hangfire.");

// --------------------------------------------
//  Run
// --------------------------------------------
app.Run();

// --------------------------------------------
//  Método auxiliar con Scope
// --------------------------------------------
 static async Task ExecuteDbJobScoped(IServiceProvider serviceProvider)
{
    using var scope = serviceProvider.CreateScope();
    var dbJob = scope.ServiceProvider.GetRequiredService<DbJob>();
    await dbJob.Execute();
}
//[2025-10-31T20:13:22.8639058Z] ["add", "1", "12/11/2025"]
//["add", "$", "USD", "dollar", "1"]
//["add", "ilias", "0", "123"]
//["add", "2", "2", "1"]