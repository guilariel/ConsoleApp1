using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RabbitMQAndGenericRepository.Repositorio.DbEntities
{
    public class PriceHistoryDb : IEFEntity<int>
    {
        [NotMapped]
        public int key => stock_id;
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int stock_id { get; private set; }
        public double price { get; set; }
        public string currency {  get; set; }
        public DateTime date { get; set; }

    }
}
