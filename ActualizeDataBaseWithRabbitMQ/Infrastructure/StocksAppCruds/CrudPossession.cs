using Microsoft.EntityFrameworkCore;
using ActualizeDataBaseWithRabbitMQ.Domain.Entities;

namespace ActualizeDataBaseWithRabbitMQ.Infrastructure.StocksAppCruds
{
    public class StocksAppCrudPossession
    {
        private readonly StocksAppDbContext _context;
        private readonly StocksAppCrudUsers _crudUsers;

        public StocksAppCrudPossession(StocksAppDbContext context, StocksAppCrudUsers crudUsers)
        {
            _context = context;
            _crudUsers = crudUsers;
        }

        // Obtener todos
        public async Task<List<InPossessionDb>> GetAllAsync()
        {
            return await _context.in_possession.ToListAsync();
        }

        // Obtener uno específico (por owner_id y symbol_id)
        public async Task<InPossessionDb?> GetOneAsync(int ownerId, int symbolId)
        {
            return await _context.in_possession.FirstOrDefaultAsync(
                i => i.owner_id == ownerId && i.stock_id == symbolId
            );
        }
        public async Task<List<InPossessionDb>> GetPossessionsByOwnerId(int ownerId)
        {
            return await _context.in_possession
                .Where(i => i.owner_id == ownerId)
                .ToListAsync();
        }

        public async Task<List<InPossessionDb>> GetPossessionsByOwner(string ownerName)
        {
            List<InPossessionDb> possessions = await GetAllAsync();
            UsersDb? owner = await _crudUsers.GetOneByNameAsync(ownerName);
            return await _context.in_possession.Where(i => i.owner_id == owner.id).ToListAsync();
        }

        // Eliminar registro
        public async Task DeleteAsync(int ownerId, int symbolId, int amount)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var possession = await _context.in_possession.FirstOrDefaultAsync(
                    i => i.owner_id == ownerId && i.stock_id == symbolId
                );
                StockDb stockDb = await _context.stock.FirstOrDefaultAsync(s => s.id == symbolId);
                PriceDb price = await _context.price.FirstOrDefaultAsync(p => p.id == stockDb.price_id);


                if (possession == null)
                    throw new Exception("Record not found");
                if (possession.amount <= amount)
                {
                    await _crudUsers.SellFundsAsync(possession.owner_id, price.price * possession.amount);
                    _context.in_possession.Remove(possession);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    possession.amount -= amount;
                    await _crudUsers.SellFundsAsync(possession.owner_id, price.price * amount);
                    _context.in_possession.Update(possession);
                    await _context.SaveChangesAsync();
                }
                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine("ERROR DeleteAsync possession: " + ex.Message);
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task AddPossession(InPossessionDb possession)
        {
            var existingPossession = await GetOneAsync(possession.owner_id, possession.stock_id);
            UsersDb usersDb = await _crudUsers.GetByIdAsync(possession.owner_id);
            StockDb stockDb = await _context.stock.FirstOrDefaultAsync(s => s.id == possession.stock_id);
            PriceDb PriceDb = await _context.price.FirstOrDefaultAsync(i => i.id == stockDb.price_id);
            if (usersDb.funds < possession.amount * PriceDb.price)
                throw new Exception("Insufficient funds");
            if (existingPossession != null)
            {
                existingPossession.amount += possession.amount; // Actualizar la cantidad existente
                _context.in_possession.Update(existingPossession); // Actualizar el registro en la base de datos
            }
            else
            {
                _context.in_possession.Add(possession); // Agregar un nuevo registro
            }
            await _crudUsers.UpdateFundsAsync(possession.owner_id, possession.amount * PriceDb.price); // Aquí puedes ajustar el costo si es necesario
            await _context.SaveChangesAsync();
        }
    }
}
