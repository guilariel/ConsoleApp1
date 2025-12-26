using ActualizeDataBaseWithRabbitMQ.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using RabbitMQAndGenericRepository.Repositorio;

namespace ActualizeDataBaseWithRabbitMQ.Infrastructure
{
    public class StocksAppUnitOfWork : UnitOfWorkBase
    {
        public InPossessionRepository InPossessionRepository { get; }
        public PriceRepository PriceRepository { get; }
        public StockRepository StockRepository { get; }
        public TransactionRepository TransactionRepository { get; }
        public UserRepository UserRepository { get; }
        public UserFundsRepository UserFundsRepository { get; }

        public StocksAppUnitOfWork(IConfiguration config)
            : base(config.GetConnectionString("StocksApp"))
        {

            InPossessionRepository = new InPossessionRepository(_context);
            PriceRepository = new PriceRepository(_context);
            StockRepository = new StockRepository(_context);
            TransactionRepository = new TransactionRepository(_context);
            UserRepository = new UserRepository(_context);
            UserFundsRepository = new UserFundsRepository(_context);
        }
    }

}
