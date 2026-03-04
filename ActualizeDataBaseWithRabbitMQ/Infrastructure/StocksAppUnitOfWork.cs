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

        public StocksAppUnitOfWork(StocksAppDbContext dbContext) : base(dbContext)
        {
            PriceRepository = new PriceRepository(dbContext);
            StockRepository = new StockRepository(dbContext);
            UserRepository = new UserRepository(dbContext);
            TransactionRepository = new TransactionRepository(dbContext);
            UserFundsRepository = new UserFundsRepository(dbContext);
            InPossessionRepository = new InPossessionRepository(dbContext);
        }
    }

}
