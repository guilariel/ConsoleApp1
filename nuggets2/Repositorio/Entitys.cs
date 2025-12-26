using System.Xml.Linq;

namespace RabbitMQAndGenericRepository.Repositorio
{
    public interface IEntitys<T>
    {
        T Key { get; }
    }
}
