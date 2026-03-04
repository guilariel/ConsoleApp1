using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RabbitMQAndGenericRepository.Repositorio;
using RabbitMQAndGenericRepository.Repositorio.DbEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ActualizeDataBaseWithRabbitMQ.Infrastructure
{
    public class StocksAppDbContext : DbContext
    {
        public DbSet<InPossessionDb> in_possession { get; set; }
        public DbSet<PriceHistoryDb> price_history { get; set; }
        public DbSet<StockDb> stock { get; set; }
        public DbSet<UsersDb> users { get; set; }
        public DbSet<UserFundsDb> user_funds { get; set; }
        public DbSet<TransactionHistoryDb> transaction_history { get; set; }
        public DbSet<ProcessedMessages> processed_messages { get; set; }
        public StocksAppDbContext(DbContextOptions <StocksAppDbContext> options)
            : base( options) { }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<InPossessionDb>()
            .HasKey(x => new { x.owner_id, x.stock_id });

            modelBuilder.Entity<UserFundsDb>()
                .HasKey(x => new { x.user_id, x.currency });

            modelBuilder.Entity<PriceHistoryDb>()
                .HasKey(p => new { p.stock_id, p.date });

        }
    }

    public class SellStocksDbContext : DbContext
    {
        public DbSet<PriceHistoryDb> price_history { get; set; }
        public DbSet<StockDb> stock { get; set; }
        public DbSet<UsersDb> users { get; set; }
        public SellStocksDbContext(DbContextOptions<SellStocksDbContext> options)
            : base(options) { }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        { 
            modelBuilder.Entity<PriceHistoryDb>()
                .HasKey(p => new { p.stock_id, p.date });
        }
    }

    public class PurchaseStocksDbContext : DbContext
    {
        public DbSet<PriceHistoryDb> price_history { get; set; }
        public DbSet<StockDb> stock { get; set; }
        public DbSet<UsersDb> users { get; set; }
        public PurchaseStocksDbContext(DbContextOptions<PurchaseStocksDbContext> options)
          : base(options) { }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PriceHistoryDb>()
                .HasKey(p => new { p.stock_id, p.date });
        }
    }

    public class LogInDbContext : IdentityDbContext<IdentityUser>
    {
        public DbSet<UserFundsDb> userFunds { get; set; } 
        public LogInDbContext(DbContextOptions<LogInDbContext> options)
            : base(options) { }
    }

}
