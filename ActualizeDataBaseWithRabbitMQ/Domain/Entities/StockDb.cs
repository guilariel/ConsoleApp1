using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ActualizeDataBaseWithRabbitMQ.Domain.Entities
{
    public class StockDb
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id {  get; set; }
        public string symbol { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public int price_id { get; set; }

    }
}
