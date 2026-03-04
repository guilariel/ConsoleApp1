using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQAndGenericRepository.Repositorio.DbEntities
{
    public class ProcessedMessages
    {

        [Key]
        public string message_Id { get; set; }
        public DateTime procesedAt { get; set; }
    }
}
