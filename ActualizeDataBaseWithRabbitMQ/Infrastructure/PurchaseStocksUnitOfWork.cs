using ActualizeDataBaseWithRabbitMQ.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using RabbitMQAndGenericRepository.Repositorio;

namespace ActualizeDataBaseWithRabbitMQ.Infrastructure
{
    public class PurchaseStocksUnitOfWork : UnitOfWorkBase
    {
        public readonly PriceRepository PriceRepository;
        public readonly StockRepository StockRepository;
        public readonly UserRepository UserRepository;
        public PurchaseStocksUnitOfWork(PurchaseStocksDbContext dbContext) : base(dbContext)
        {
            PriceRepository = new PriceRepository(dbContext);
            StockRepository = new StockRepository(dbContext);
            UserRepository = new UserRepository(dbContext);
        }
    }
}
