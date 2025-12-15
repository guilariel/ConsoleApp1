using System;
using System.Text;
using RabbitMQ.Client;
using System.Text.Json;
using RabbitMQAndGenericRepository.RabbitMq;
using RabbitMQAndGenericRepository.Repositorio;


namespace RabbitMQAndGenericRepository.RabbitMq
{
    public class RabbitProducer 
    {
        private readonly RabbitMQOptions options;
        public RabbitProducer(RabbitMQOptions rabbitMQOptions)
        {
            options = rabbitMQOptions;
        }
        public async Task SendMessage<T>(T data, string task) where T : class, new()
        {
            var factory = new ConnectionFactory{ Uri = new Uri (options.Amqp), AutomaticRecoveryEnabled = true, ClientProvidedName = "producer"  };
            var conection = await factory.CreateConnectionAsync();
            var channel = await conection.CreateChannelAsync();

            List<string> dataString = new List<string>();
            Type type = typeof(T);
            if (task == "add" || task == "sell")
            {
                dataString.Add(task);
            }
            foreach (var prop in type.GetProperties())
            {
                var value = prop.GetValue(data);
                if (value != null) 
                dataString.Add(value.ToString());
            }
            var body = JsonSerializer.Serialize(dataString.ToArray());
            var text = Encoding.UTF8.GetBytes(body);
            await channel.BasicPublishAsync("",options.QueueName,false,text);
            await channel.CloseAsync();
        }
    }
}
