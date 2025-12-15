using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RabbitMQAndGenericRepository.Repositorio.DbEntities
{
    public class InPossessionDb : IEFEntity
    {
        [Key]
        [Column("owner_id")]
        public int id { get; set; }
        public int stock_id { get; set; }
        public int amount { get; set; }

    }
}
