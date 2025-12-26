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
    interface IPriceRepository : IRepository<PriceHistoryDb, int>
    {
        Task<PriceHistoryDb> GetLatestAsync(int id);
    }
    public class PriceRepository : EFRepository<PriceHistoryDb, int>, IPriceRepository
    {
        public PriceRepository(DbContext context) : base(context)
        {
        }
        public async Task<PriceHistoryDb> GetLatestAsync(int id)
        {
            return await _entities
                .Where(e => e.key == id)
                .OrderByDescending(e => e.date)
                .FirstOrDefaultAsync();
        }
    }
}
