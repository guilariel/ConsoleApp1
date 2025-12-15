using Microsoft.EntityFrameworkCore;
using ActualizeDataBaseWithRabbitMQ.Domain.Entities;
namespace ActualizeDataBaseWithRabbitMQ.Infrastructure
{
    public class SellStocksDbContext : DbContext
    {
        public DbSet<InPossessionDb> in_possession { get; set; }
        public DbSet<PriceDb> price { get; set; }
        public DbSet<StockDb> stock { get; set; }
        public DbSet<UsersDb> users { get; set; }
        public SellStocksDbContext(DbContextOptions<SellStocksDbContext> options) : base(options) { }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<InPossessionDb>()
                .HasKey(p => new { p.owner_id, p.stock_id });  
        }
    }
}
