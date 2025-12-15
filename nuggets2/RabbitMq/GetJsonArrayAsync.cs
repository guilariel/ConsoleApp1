using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace RabbitMQAndGenericRepository.RabbitMq
{
    internal class GetJsonArrayAsync
    {
        private readonly RabbitMQOptions options;
        public GetJsonArrayAsync(RabbitMQOptions options)
        {
            this.options = options;
        }
        public async Task<List<string>> GetMessagesAsync()
        {
            List<string> messages = new List<string>();
            var factory = new ConnectionFactory
            {
                Uri = new Uri(options.Amqp),
                AutomaticRecoveryEnabled = true,
                ClientProvidedName = "listener-console"
            };
            await using var connection = await factory.CreateConnectionAsync();
            await using var channel = await connection.CreateChannelAsync();

            try
            {
                await channel.QueueDeclarePassiveAsync(options.QueueName);
            }
            catch (RabbitMQ.Client.Exceptions.OperationInterruptedException ex) when (ex.ShutdownReason?.ReplyCode == 404)
            {
                Console.Error.WriteLine($"[404] La cola '{options.QueueName}' no existe. Creala en la UI o por policy.");
                return null;
            }

            var result = await channel.BasicGetAsync("zeka",autoAck: true);

            while (result != null)
            {
                var body = result.Body.ToArray();
                var text = Encoding.UTF8.GetString(body);
                messages.Add(text);
                result = await channel.BasicGetAsync("zeka", autoAck: true);
            }

            return messages;
        }
    }
}

/*            consumer.ReceivedAsync += async (_, ea) =>
            {
                try
                {
                    var body = ea.Body.ToArray();
                    var text = Encoding.UTF8.GetString(body);
                    messages.Add(text);    
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"[ERR] {ex.Message}");
                    await channel.BasicNackAsync(ea.DeliveryTag, false, requeue: true);
                }
            };*/