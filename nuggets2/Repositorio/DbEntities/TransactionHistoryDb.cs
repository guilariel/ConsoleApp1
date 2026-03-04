using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQAndGenericRepository.Repositorio.DbEntities
{
    public class TransactionHistoryDb 
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string id { get; set; }
        public string owner_id { get; set; }
        public string stock_id { get; set; }
        public double amount { get; set; }
        public double price { get; set; }
        public string currency { get; set; }
        public DateTime date { get; set; }
        public string type { get; set; }
    }
}
