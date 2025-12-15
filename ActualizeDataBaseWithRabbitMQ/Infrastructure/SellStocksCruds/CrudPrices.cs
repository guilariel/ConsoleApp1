using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using ActualizeDataBaseWithRabbitMQ.Domain.Entities;

namespace ActualizeDataBaseWithRabbitMQ.Infrastructure.SellStocksCruds
{
    public class SellStocksCrudPrices
    {
        private readonly SellStocksDbContext appDbContext;
        public SellStocksCrudPrices(SellStocksDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }

        public async Task AddPrice(PriceDb priceDbDto)
        {
            appDbContext.Add(priceDbDto);
            await appDbContext.SaveChangesAsync();
        }
        public async Task UpdatePrice(PriceDb priceDbDto)
        {
            var existing = await appDbContext.price.FirstOrDefaultAsync(p => p.id == priceDbDto.id);
            if (existing == null)
                throw new Exception("Price not found");

            existing.date = priceDbDto.date;
            existing.price = priceDbDto.price; 
            await appDbContext.SaveChangesAsync();

        }
        public async Task<List<PriceDb>> GetAllPrices()
        {
             var prices = await appDbContext.price.ToListAsync();           
             return prices;   
        }
        public async Task<PriceDb> GetOnePrice(int id)
        {
            var price = await appDbContext.price.FirstOrDefaultAsync(u => u.id == id);
            if (price == null)
                throw new Exception("there´s no id for that price");
            return price;    
        }

        public async Task DeleteOnePrice(int id)
        {
            var price = await appDbContext.price.FirstOrDefaultAsync(p => p.id == id);
            if (price == null)
                throw new Exception("Price not found");

            appDbContext.Remove(price);
            await appDbContext.SaveChangesAsync();
        }
    }
}
