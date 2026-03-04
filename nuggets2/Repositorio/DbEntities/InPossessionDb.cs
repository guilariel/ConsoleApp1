using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RabbitMQAndGenericRepository.Repositorio.DbEntities
{
    public class InPossessionDb 
    {
   
        public string owner_id { get; set; }
        public string stock_id { get; set; }
        public int amount { get; set; }
    }
    public readonly record struct InPossessionStruct(int owner_id, int stock_id);
}
