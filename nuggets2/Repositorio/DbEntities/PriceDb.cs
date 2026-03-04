using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RabbitMQAndGenericRepository.Repositorio.DbEntities
{
    public class PriceHistoryDb 
    {
        public string stock_id { get; set; }
        public double price { get; set; }
        public string currency { get; set; }
        public DateTime date { get; set; }

    }
}
