using Microsoft.EntityFrameworkCore;
using ActualizeDataBaseWithRabbitMQ.Domain.Entities;

namespace ActualizeDataBaseWithRabbitMQ.Infrastructure.LogInCrud
{
    public class PurchaseStocksCrudUsers
    {
        private readonly PurchaseStocksDbContext _context;

        public PurchaseStocksCrudUsers(PurchaseStocksDbContext context)
        {
            _context = context;
        }

        // Obtener todos los usuarios
        public async Task<List<UsersDb>> GetAllAsync()
        {
            return await _context.users.ToListAsync();
        }

        // Obtener usuario por ID
        public async Task<UsersDb?> GetByIdAsync(int id)
        {
            return await _context.users.FirstOrDefaultAsync(u => u.id == id);
        }
        public async Task<UsersDb?>  GetOneByNameAsync(string name)
        {
            return await _context.users.FirstOrDefaultAsync(u => u.name == name);
        }

        // Añadir usuario
        public async Task AddAsync(UsersDb user)
        {
            UsersDb? userDb = await GetOneByNameAsync(user.name);
            if (userDb == null)
            {
                _context.users.Add(user);
                await _context.SaveChangesAsync();
            }
            else
            {
                await SellFundsAsync(userDb.id, user.funds);
            }
        }

        // Actualizar usuario
        public async Task UpdateFundsAsync(int id, double cost)
        {
            var existing = await _context.users.FirstOrDefaultAsync(u => u.id == id);
            if (existing == null)
                throw new Exception("User not found");

            existing.funds -= cost;

            await _context.SaveChangesAsync();
        }
        public async Task SellFundsAsync(int id, double cost)
        {
            var existing = await _context.users.FirstOrDefaultAsync(u => u.id == id);
            if (existing == null)
                throw new Exception("User not found");

            existing.funds += cost;

            await _context.SaveChangesAsync();
        }


        // Eliminar usuario
        public async Task DeleteAsync(int id)
        {
            var user = await _context.users.FirstOrDefaultAsync(u => u.id == id);
            if (user == null)
                throw new Exception("User not found");

            _context.users.Remove(user);
            await _context.SaveChangesAsync();
        }
    }
}
