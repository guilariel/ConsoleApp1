using System.ComponentModel.DataAnnotations;

namespace ActualizeDataBaseWithRabbitMQ.Domain.Entities
{ 
    public class InPossessionDb
    {
        public int owner_id { get; set; }
        public int stock_id { get; set; }
        public int amount { get; set; }

    }
}
