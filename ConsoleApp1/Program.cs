using System; 
using ConsoleApp1;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Threading.Tasks;

class Program
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
                services.AddSingleton <RabbitMqConsumer>();
            })
            .Build();

        // Resolver el consumidor y arrancarlo
        var consumer = host.Services.GetRequiredService<RabbitMqConsumer>();
        await consumer.StartAsync();

        await host.RunAsync();
    }
}
//crear un componente producer(manda) consumer(escucha) donde el producer envie el mensaje y el consumer ejecute ese codigo gay anal
//crear un IMessageService que meta una implementacion de rabbit
//ioption para manejar appsetings
//hacer un servicio .net que escuche 
//estudiar como se manejan los nuget y crear un nuget que maneje los mensajes
//empaquetar todo en un mismo nuget(repopsitorio y producer/consumer)


/*1. Crear un componente Producer/Consumer

Producer (Productor): es quien envía mensajes (por ejemplo, “procesa esta tarea”).

Consumer (Consumidor): es quien recibe y ejecuta la tarea enviada por el producer.
 En tu caso, se pide que el consumer ejecute el código que reciba como mensaje (aunque ojo: ejecutar código directamente puede ser riesgoso, normalmente se mandan datos o instrucciones, no código arbitrario).

 En resumen:
Un Producer manda mensajes → Un Consumer los escucha y actúa en consecuencia.

2. Crear un IMessageService que use RabbitMQ

IMessageService sería una interfaz que defina métodos como:

SendMessage(string message)

ReceiveMessage()

Luego haces una implementación concreta usando RabbitMQ (que es un broker de mensajería muy usado en sistemas distribuidos).
 Esto te permite desacoplar tu aplicación: trabajas contra la interfaz, no directamente contra RabbitMQ.

3. Usar IOptions para manejar appsettings.json

En .NET, IOptions<T> te sirve para leer configuraciones desde appsettings.json.

Ejemplo: si en appsettings.json tienes:

{
  "RabbitMQ": {
    "Host": "localhost",
    "Port": 5672
  }
}


puedes mapearlo a una clase RabbitMQOptions y accederlo en tu servicio.
 Esto evita hardcodear valores.

4. Hacer un servicio .NET que escuche

Aquí se refiere a crear un Background Service (usando IHostedService o BackgroundService) que esté escuchando mensajes en RabbitMQ de forma continua.

Así, cada vez que llegue un mensaje, el consumer lo procesa.

5. Estudiar cómo se manejan los NuGet y crear uno

NuGet es el sistema de paquetes de .NET.

Aprenderás:

Cómo crear un proyecto tipo Class Library.

Empaquetarlo con dotnet pack.

Publicarlo en un feed local o en NuGet.org.

 La idea es que empaquetes tu lógica de mensajes en un NuGet reutilizable.

6. Empaquetar todo en un solo NuGet

Finalmente, juntar:

Repositorio (código base).

Producer/Consumer.

Servicios configurados.

Y publicarlo como un único NuGet que cualquier proyecto pueda instalar para usar tu sistema de mensajería.

En pocas palabras:
Estás armando un mini framework de mensajería en .NET con RabbitMQ, configurable vía appsettings.json, empaquetado en un NuGet para ser usado en otros proyectos.*/