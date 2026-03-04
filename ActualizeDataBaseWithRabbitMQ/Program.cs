using ActualizeDataBaseWithRabbitMQ.Infrastructure;
using Hangfire;
using Hangfire.PostgreSql;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQAndGenericRepository.RabbitMq;
using RabbitMQAndGenericRepository.Repositorio;
using ActualizeDataBaseWithRabbitMQ.Repositories;

var builder = WebApplication.CreateBuilder(args);
builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenLocalhost(5004); // Fuerza que siempre use el puerto 5004
});
var services = builder.Services;
var configuration = builder.Configuration;


builder.Services.AddDbContext<StocksAppDbContext>(options =>
    options.UseNpgsql(configuration.GetConnectionString("StockAppDb"))
);
builder.Services.AddDbContext<SellStocksDbContext>(options =>
    options.UseNpgsql(configuration.GetConnectionString("SellStocksDb"))
);
builder.Services.AddDbContext<PurchaseStocksDbContext>(options =>
    options.UseNpgsql(configuration.GetConnectionString("PurchaseStocksDb"))
);
builder.Services.AddDbContext<LogInDbContext>(options =>
    options.UseNpgsql(configuration.GetConnectionString("LogInDb"))
);

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

services.AddTransient<StockRepository>();
services.AddTransient<PriceRepository>();
services.AddTransient<TransactionRepository>();
services.AddTransient<UserFundsRepository>();
services.AddTransient<UserRepository>();
services.AddTransient<InPossessionRepository>();
services.AddTransient<MessageRepository>();

services.AddTransient<StocksAppUnitOfWork>();
services.AddTransient<SellStocksUnitOfWork>();
services.AddTransient<PurchaseStocksUnitOfWork>(); 
services.AddTransient<IdentityService>();

services.AddTransient<AddStockCommand>();
services.AddTransient<AddPriceCommand>();
services.AddTransient<AddTransactionCommand>();
services.AddTransient<AddUserFundsCommand>();
services.AddTransient<AddUserCommand>();
services.AddTransient<AddInPossessionCommand>();

services.AddTransient<DeleteStockCommand>();
services.AddTransient<DeletePriceCommand>();
services.AddTransient<DeleteTransactionCommand>();
services.AddTransient<DeleteUserFundsCommand>();
services.AddTransient<DeleteUserCommand>();
services.AddTransient<DeleteInPossessionCommand>();

services.AddTransient<CreateStock>();   
services.AddTransient<CreatePrice>();
services.AddTransient<CreateTransactionHistory>();
services.AddTransient<CreateUserFunds>();
services.AddTransient<CreateUser>();
services.AddTransient<CreateInPossession>();

services.AddSingleton<ICommandResolver, CommandResolver>();
services.AddSingleton<IObjectCreatorResolver, ObjectCreatorResolver>();
services.AddScoped<Idepomtecy>();

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


/* .Services.AddDbContext<StocksAppDbContext>(options =>
    options.UseNpgsql(configuration.GetConnectionString("StockApp"))
);

builder.Services.AddDbContext<SellStocksDbContext>(options =>
    options.UseNpgsql(configuration.GetConnectionString("SellStocksDb"))
);

builder.Services.AddDbContext<PurchaseStocksDbContext>(options =>
    options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"))
);

builder.Services.AddDbContext<LogInDbContext>(options =>
    options.UseNpgsql(configuration.GetConnectionString("LogInDb"))
);*/
