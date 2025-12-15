using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ActualizeDataBaseWithRabbitMQ.Domain.Entities
{
    public class PriceDb
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id {  get; set; }
        public double price { get; set; }
        private DateTime _date;
        public DateTime date
        {
            get => _date;
            set => _date = DateTime.SpecifyKind(value, DateTimeKind.Utc); 
        }

    }
}
