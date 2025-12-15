using Microsoft.EntityFrameworkCore;
using ActualizeDataBaseWithRabbitMQ.Domain.Entities;

namespace ActualizeDataBaseWithRabbitMQ.Infrastructure
{
    public class PurchaseStocksDbContext : DbContext
    {
        public PurchaseStocksDbContext(DbContextOptions<PurchaseStocksDbContext> options) : base(options) { }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<InPossessionDb>()
                .HasKey(p => new { p.owner_id, p.stock_id });  
        }

        public DbSet<StockDb> stock { get; set; }
        public DbSet<PriceDb> price { get; set; }
        public DbSet<UsersDb> users { get; set; }
        public DbSet<InPossessionDb> in_possession { get; set; }
    }
}
