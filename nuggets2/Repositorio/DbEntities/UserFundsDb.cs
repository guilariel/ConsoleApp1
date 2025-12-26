using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQAndGenericRepository.Repositorio.DbEntities
{
    public class UserFundsDb: IEFEntity<UserFundsStruct>
    {
        [NotMapped]
        public UserFundsStruct key => new UserFundsStruct(user_id, currency);
        public int user_id { get; set; }
        public double funds { get; set; }
        public string currency { get; set; }
    }
}
