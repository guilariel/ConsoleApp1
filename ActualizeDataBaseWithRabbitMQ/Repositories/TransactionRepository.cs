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
    internal interface ITransactionRepository : IRepository<TransactionHistoryDb, int>
    {
        Task<TransactionHistoryDb> GetByOwnerAndStockAsync(InPossessionStruct inPossessionStruct);
    }
    public class TransactionRepository : EFRepository<TransactionHistoryDb, int>, ITransactionRepository
    {
        public TransactionRepository(DbContext context) : base(context)
        {
        }
        public async Task<TransactionHistoryDb> GetByOwnerAndStockAsync(InPossessionStruct inPossession)
        {
            return await _entities.FirstOrDefaultAsync(t => t.owner_id == inPossession.owner_id && t.stock_id == inPossession.stock_id);
        }
        public async Task<List<TransactionHistoryDb>> GetByOwnerIdAsync(int owner_id)
        {
            return await _entities.Where(t => t.owner_id == owner_id).ToListAsync();
        }
    }
}
