using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RabbitMQAndGenericRepository.Repositorio.DbEntities
{
    public class PriceHistoryDb 
    {
        public int key => stock_id;
        public int stock_id { get; set; }
        public double price { get; set; }
        public string currency { get; set; }
        public DateTime date { get; set; }

    }
}
