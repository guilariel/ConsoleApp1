using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RabbitMQAndGenericRepository.Repositorio.DbEntities
{
    public class StockDb 
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string id { get; set; }
        public string symbol { get; set; }
        public string name { get; set; }
        public string description { get; set; }

    }
}
