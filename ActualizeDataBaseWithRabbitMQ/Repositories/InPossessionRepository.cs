using ActualizeDataBaseWithRabbitMQ.Infrastructure;
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
    internal interface IInPossessionRepository : IRepository<InPossessionDb>
    {
        Task<List<InPossessionDb>> GetAllByOwnerAsync(string owner_id);
    }
    public class InPossessionRepository : EFRepository<InPossessionDb>, IInPossessionRepository
    {
        public InPossessionRepository(DbContext context) : base(context)
        {
        }
        public async Task<List<InPossessionDb>> GetAllByOwnerAsync(string owner_id)
        {
            return await _entities.Where(id => id.owner_id == owner_id).ToListAsync();
        }
        public override async Task AddAsync(InPossessionDb entity)
        {
            InPossessionDb? existing = await _entities.FirstOrDefaultAsync(ip => ip.owner_id == entity.owner_id && ip.stock_id == entity.stock_id);
            if (existing != null)
            {
                existing.amount += entity.amount; // Update the tracked entity
                await UpdateAsync(existing);      // Pass the tracked entity
                return;
            }
            await base.AddAsync(entity);
        }
        public override async Task DeleteAsync(InPossessionDb entity)
        {
            var existing = await _entities.FirstOrDefaultAsync(ip =>
             ip.owner_id == entity.owner_id &&
             ip.stock_id == entity.stock_id);

            if (existing == null)
                return;

            if (existing.amount > entity.amount)
            {
                existing.amount -= entity.amount;
                await UpdateAsync(existing);
            }
            else
            {
                _entities.Remove(existing);
            }
        }
    }
}
