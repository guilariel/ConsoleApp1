using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RabbitMQAndGenericRepository.Repositorio.DbEntities
{
    public class InPossessionDb 
    {
   
        public int owner_id { get; set; }
        public int stock_id { get; set; }
        public int amount { get; set; }
    }
    public readonly record struct InPossessionStruct(int owner_id, int stock_id);
}
