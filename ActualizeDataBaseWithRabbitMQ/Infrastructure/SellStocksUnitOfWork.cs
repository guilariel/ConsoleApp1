using ActualizeDataBaseWithRabbitMQ.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using RabbitMQAndGenericRepository.Repositorio;

namespace ActualizeDataBaseWithRabbitMQ.Infrastructure
{
    public class SellStocksUnitOfWork : UnitOfWorkBase
    {
        public readonly PriceRepository PriceRepository;
        public readonly StockRepository StockRepository;
        public readonly UserRepository UserRepository;
        public SellStocksUnitOfWork(SellStocksDbContext dbContext) : base(dbContext)
        {
            PriceRepository = new PriceRepository(dbContext);
            StockRepository = new StockRepository(dbContext);
            UserRepository = new UserRepository(dbContext);
        }
    }
}
