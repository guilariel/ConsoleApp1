using RabbitMQAndGenericRepository.Repositorio;
using System.Text.Json.Nodes;

namespace RabbitMQAndGenericRepository.RabbitMq
{
    public interface IMessageService
    {
        Task SendMessage<T>(T data, string task) where T : class, new();
        Task ConsumeMessages();
        Task<List<T>> GetAsync<T>() where T : class, new();
    }
}
