using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQAndGenericRepository.RabbitMq
{
    public class RabbitMQOptions
    {
        public string Host { get; set; } = string.Empty;
        public int Port { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Amqp { get; set; } = string.Empty;
        public string QueueName { get; set; } = string.Empty;

    public static RabbitMQOptions ParseRabbitOptions(string? connectionString)
        {
            if (string.IsNullOrWhiteSpace(connectionString))
                throw new InvalidOperationException("No se encontró la variable de entorno ConnectionString_Rabbit.");

            var options = new RabbitMQOptions();
            var pairs = connectionString.Split(';', StringSplitOptions.RemoveEmptyEntries);

            foreach (var pair in pairs)
            {
                var parts = pair.Split('=', 2);
                if (parts.Length != 2) continue;

                var key = parts[0].Trim();
                var value = parts[1].Trim();

                switch (key)
                {
                    case "Host":
                        options.Host = value;
                        break;
                    case "Port":
                        if (int.TryParse(value, out int port))
                            options.Port = port;
                        break;
                    case "Username":
                        options.Username = value;
                        break;
                    case "Password":
                        options.Password = value;
                        break;
                    case "Amqp":
                        options.Amqp = value;
                        break;
                    case "QueueName":
                        options.QueueName = value;
                        break;
                }
            }
            return options;
        }
    } 
}
