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
        public PurchaseStocksUnitOfWork(IConfiguration config)
            : base(config.GetConnectionString("DefaultConnection"))
        {
            PriceRepository = new PriceRepository(_context);
            StockRepository = new StockRepository(_context);
            UserRepository = new UserRepository(_context);
        }
    }
}
