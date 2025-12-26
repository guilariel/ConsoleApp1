using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQAndGenericRepository.Repositorio.DbEntities
{
    public class TransactionHistoryDb : IEFEntity<int>
    {
        [NotMapped]
        public int key => id;
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; private set; }
        public int owner_id { get; set; }
        public int stock_id { get; set; }
        public int amount { get; set; }
        public double price { get; set; }
        public string currency { get; set; }
        public DateTime date { get; set; }
        public string type { get; set; }
    }
}
