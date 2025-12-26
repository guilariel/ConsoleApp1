using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RabbitMQAndGenericRepository.Repositorio.DbEntities
{
    public class InPossessionDb : IEFEntity<InPossessionStruct>
    {
        [NotMapped]
        public InPossessionStruct key => new InPossessionStruct(owner_id, stock_id);     
   
        public int owner_id { get; set; }
        public int stock_id { get; set; }
        public int amount { get; set; }
    }
}
