using Microsoft.EntityFrameworkCore;
using RabbitMQAndGenericRepository.Repositorio;
using RabbitMQAndGenericRepository.Repositorio.DbEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ActualizeDataBaseWithRabbitMQ.Repositories
{
    internal interface IInPossessionRepository : IRepository<InPossessionDb, InPossessionStruct>
    {
        Task<List<InPossessionDb>> GetAllByOwnerAsync(int owner_id);
    }
    public class InPossessionRepository : EFRepository<InPossessionDb, InPossessionStruct>, IInPossessionRepository
    {
        public InPossessionRepository(DbContext context) : base(context)
        {
        }
        public async Task<List<InPossessionDb>> GetAllByOwnerAsync(int owner_id)
        {
            return await _entities.Where(id => id.owner_id == owner_id).ToListAsync();
        }
    }
}
