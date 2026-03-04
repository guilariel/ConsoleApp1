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
    internal interface ITransactionRepository : IRepository<TransactionHistoryDb>
    {
        Task<TransactionHistoryDb> GetByOwnerAndStockAsync(string owner_id, string stock_id);
    }
    public class TransactionRepository : EFRepository<TransactionHistoryDb>, ITransactionRepository
    {
        public TransactionRepository(DbContext context) : base(context)
        {
        }
        public async Task<TransactionHistoryDb> GetByOwnerAndStockAsync(string owner_id, string stock_id)
        {
            return await _entities.FirstOrDefaultAsync(t => t.owner_id == owner_id && t.stock_id == stock_id);
        }
        public async Task<List<TransactionHistoryDb>> GetByOwnerIdAsync(string owner_id)
        {
            return await _entities.Where(t => t.owner_id == owner_id).ToListAsync();
        }
    }
}
