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
    interface IPriceRepository : IRepository<PriceHistoryDb>
    {
        Task<PriceHistoryDb> GetLatestAsync(string id);
    }
    public class PriceRepository : EFRepository<PriceHistoryDb>, IPriceRepository
    {
        public PriceRepository(DbContext context) : base(context)
        {
        }
        public async Task<PriceHistoryDb> GetLatestAsync(string id)
        {
            return await _entities
                .Where(e => e.stock_id == id)
                .OrderByDescending(e => e.date)
                .FirstOrDefaultAsync();
        }
    }
}
