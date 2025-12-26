using Microsoft.EntityFrameworkCore;
using RabbitMQAndGenericRepository.Repositorio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ActualizeDataBaseWithRabbitMQ.Infrastructure
{
    public class StocksAppDbContext : GenericDbContext
    {
        public StocksAppDbContext(DbContextOptions <GenericDbContext> options)
            : base( options) { }
    }

    public class SellStocksDbContext : GenericDbContext
    {
        public SellStocksDbContext(DbContextOptions<GenericDbContext> options)
            : base(options) { }
    }

    public class PurchaseStocksDbContext : GenericDbContext
    {
        public PurchaseStocksDbContext(DbContextOptions<GenericDbContext> options)
            : base(options) { }
    }

    public class LogInDbContext : GenericDbContext
    {
        public LogInDbContext(DbContextOptions<GenericDbContext> options)
            : base(options) { }
    }

}
