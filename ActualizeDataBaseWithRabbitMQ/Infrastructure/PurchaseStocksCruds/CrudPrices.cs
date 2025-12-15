using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using ActualizeDataBaseWithRabbitMQ.Domain.Entities;

namespace ActualizeDataBaseWithRabbitMQ.Infrastructure.PurchaseStocks
{
    public class PurchaseStocksCrudPrices
    {
        private readonly PurchaseStocksDbContext _context;
        public PurchaseStocksCrudPrices(PurchaseStocksDbContext _context)
        {
            this._context = _context;
        }

        public async Task AddPrice(PriceDb priceDbDto)
        {
            _context.Add(priceDbDto);
            await _context.SaveChangesAsync();
        }
        public async Task UpdatePrice(PriceDb priceDbDto)
        {
            var existing = await _context.price.FirstOrDefaultAsync(p => p.id == priceDbDto.id);
            if (existing == null)
                throw new Exception("Price not found");
            existing.date = priceDbDto.date;
            existing.price = priceDbDto.price; 
            await _context.SaveChangesAsync();

        }
        public async Task<List<PriceDb>> GetAllPrices()
        {
             var prices = await _context.price.ToListAsync();           
             return prices;   
        }
        public async Task<PriceDb> GetOnePrice(int id)
        {
            var price = await _context.price.FirstOrDefaultAsync(u => u.id == id);
            if (price == null)
                throw new Exception("there´s no id for that price");
            return price;    
        }

        public async Task DeleteOnePrice(int id)
        {
            var price = await _context.price.FirstOrDefaultAsync(p => p.id == id);
            if (price == null)
                throw new Exception("Price not found");

            _context.Remove(price);
            await _context.SaveChangesAsync();
        }
    }
}
