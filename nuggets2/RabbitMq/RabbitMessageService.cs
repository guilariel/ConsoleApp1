using Microsoft.Extensions.Options;
using RabbitMQAndGenericRepository.Repositorio;


namespace RabbitMQAndGenericRepository.RabbitMq
{
    public class RabbitMessageService : IMessageService
    {
        public RabbitMQOptions Options { get; set; }
        public RabbitMessageService(RabbitMQOptions options)
        {
            Options = options;
        }

        public async Task SendMessage<T>(T data, string task) where T : class, new()
        {
            RabbitProducer producer = new RabbitProducer(Options);
            await producer.SendMessage<T>(data, task);
        }
        public async Task ConsumeMessages()
        {
            RabbitMqConsumer consumer = new RabbitMqConsumer(Options);
            await consumer.StartAsync();
        }
        public async Task<List<T>> GetAsync<T>() where T : class, new()
        {
            GetJsonArrayAsync getJsonArrayAsync = new GetJsonArrayAsync(Options);
            FilterRabbitResultes results = new FilterRabbitResultes();
            return await results.FilterResults<T>(await getJsonArrayAsync.GetMessagesAsync());
        }
        public async Task<List<string>> GetRawMessagesAsync()
        {
            GetJsonArrayAsync getJsonArrayAsync = new GetJsonArrayAsync(Options);
            return await getJsonArrayAsync.GetMessagesAsync();
        }
    }
}
//crear un componente producer(manda) consumer(escucha) donde el producer envie el mensaje y el consumer ejecute ese codigo gay anal
//crear un IMessageService que meta una implementacion de rabbit
//ioption para manejar appsetings
//hacer un servicio .net que escuche 
//estudiar como se manejan los nuget y crear un nuget que maneje los mensajes
//empaquetar todo en un mismo nuget(repopsitorio y producer/consumer)

/*class Program
{
    static async Task Main(string[] args)
    {
        // Crear Host genérico
        var host = Host.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration((context, config) =>
            {
                config.SetBasePath(AppContext.BaseDirectory)
                      .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            })
            .ConfigureServices((context, services) =>
            {
                services.Configure<RabbitMQOptions>(context.Configuration.GetSection("RabbitMQ"));
                services.AddSingleton<RabbitMqConsumer>();
            })
            .Build();

        // Resolver el consumidor y arrancarlo
        var consumer = host.Services.GetRequiredService<RabbitMqConsumer>();
        await consumer.StartAsync();

        await host.RunAsync();
    }
}.*/