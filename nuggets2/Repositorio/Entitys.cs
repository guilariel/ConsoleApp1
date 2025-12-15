using System.Xml.Linq;

namespace RabbitMQAndGenericRepository.Repositorio
{
    public interface IEntity<T>
    {
        T Key { get; }
    }
}
