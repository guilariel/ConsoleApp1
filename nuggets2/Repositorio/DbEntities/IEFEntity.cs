using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQAndGenericRepository.Repositorio.DbEntities
{

    public interface IEFEntity<T>
    {
        T key { get; }
    }
    public readonly record struct InPossessionStruct(int owner_id, int stock_id);
    public readonly record struct UserFundsStruct(int user_id, string currency);
}
