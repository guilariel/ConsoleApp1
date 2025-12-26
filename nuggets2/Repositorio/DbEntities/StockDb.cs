using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RabbitMQAndGenericRepository.Repositorio.DbEntities
{
    public class StockDb : IEFEntity<int>
    {
        [NotMapped]
        public int key => id;
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; private set; }
        public string symbol { get; set; }
        public string name { get; set; }
        public string description { get; set; }

    }
}
