using Consumer;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

public class RabbitMqConsumer
{
    private readonly RabbitMQOptions _options;

    public RabbitMqConsumer(IOptions<RabbitMQOptions> options)
    {
        _options = options.Value;
    }

    public async Task StartAsync()
    {
        ushort prefetch = 32;

        using var cts = new CancellationTokenSource();
        Console.CancelKeyPress += (_, e) => { e.Cancel = true; cts.Cancel(); };

        var factory = new ConnectionFactory
        {
            Uri = new Uri(_options.Amqp),
            AutomaticRecoveryEnabled = true,
            ClientProvidedName = "listener-console"
        };

        await using var conn = await factory.CreateConnectionAsync();
        await using var ch = await conn.CreateChannelAsync();

        try
        {
            await ch.QueueDeclarePassiveAsync(_options.QueueName);
        }
        catch (RabbitMQ.Client.Exceptions.OperationInterruptedException ex) when (ex.ShutdownReason?.ReplyCode == 404)
        {
            Console.Error.WriteLine($"[404] La cola '{_options.QueueName}' no existe. Creala en la UI o por policy.");
            return;
        }

        await ch.BasicQosAsync(0, prefetch, false);

        var consumer = new AsyncEventingBasicConsumer(ch);
        consumer.ReceivedAsync += async (_, ea) =>
        {
            try
            {
                var body = ea.Body.ToArray();
                var text = Encoding.UTF8.GetString(body);
                Console.WriteLine($"[{DateTime.UtcNow:O}] {text}");
                await ch.BasicAckAsync(ea.DeliveryTag, false);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"[ERR] {ex.Message}");
                await ch.BasicNackAsync(ea.DeliveryTag, false, requeue: true);
            }
        };

        var tag = await ch.BasicConsumeAsync(queue: _options.QueueName, autoAck: false, consumer: consumer);

        Console.WriteLine($"Escuchando cola '{_options.QueueName}' en {_options.Amqp}. Ctrl+C para salir.");

        try { while (!cts.IsCancellationRequested) await Task.Delay(250, cts.Token); }
        catch (TaskCanceledException) { }

        try { await ch.BasicCancelAsync(tag); } catch { }
    }
}
//zeka princesa hermosa