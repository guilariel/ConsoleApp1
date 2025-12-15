using Microsoft.EntityFrameworkCore;
using ActualizeDataBaseWithRabbitMQ.Domain.Entities;

namespace ActualizeDataBaseWithRabbitMQ.Infrastructure.StocksAppCruds
{
    public class StocksAppCrudStocks
    {
        private readonly StocksAppDbContext _context;

        public StocksAppCrudStocks(StocksAppDbContext context)
        {
            _context = context;
        }

        // Obtener todos
        public async Task<List<StockDb>> GetAllAsync()
        {
            return await _context.stock.ToListAsync();
        }

        // Obtener uno por ID
        public async Task<StockDb?> GetByIdAsync(int id)
        {
            return await _context.stock.FirstOrDefaultAsync(s => s.id == id);
        }
        public async Task<StockDb?> GetOneByNameAsync(string name)
        {
            return await _context.stock.FirstOrDefaultAsync(s => s.name == name);
        }
        // Añadir nuevo
        public async Task AddAsync(StockDb stock)
        {
            _context.stock.Add(stock);
            await _context.SaveChangesAsync();
        }

        // Actualizar
        public async Task UpdateAsync(StockDb stock)
        {
            var existing = await _context.stock.FirstOrDefaultAsync(s => s.id == stock.id);
            if (existing == null)
                throw new Exception("Stock not found");

            existing.symbol = stock.symbol;
            existing.name = stock.name;
            existing.description = stock.description;
            existing.price_id = stock.price_id;

            await _context.SaveChangesAsync();
        }

        // Eliminar
        public async Task DeleteAsync(int id)
        {
            var stock = await _context.stock.FirstOrDefaultAsync(s => s.id == id);
            if (stock == null)
                throw new Exception("Stock not found");

            _context.stock.Remove(stock);
            await _context.SaveChangesAsync();
        }
        public async Task<PriceDb?> GetStockPriceByNameAsync(string name)
        {
            StockDb? stockDb = await _context.stock.FirstOrDefaultAsync(s =>s.name == name);
            return await _context.price.FirstOrDefaultAsync(p => p.id == stockDb.price_id);
        }
    }
}
