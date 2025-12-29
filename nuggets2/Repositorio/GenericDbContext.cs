using Microsoft.EntityFrameworkCore; // Asegúrate de tener este using
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Metadata; // <-- Añade este using
using Microsoft.EntityFrameworkCore.Infrastructure;
using RabbitMQAndGenericRepository.Repositorio.DbEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQAndGenericRepository.Repositorio
{
    public class GenericDbContext : DbContext
    {
        public DbSet<InPossessionDb> in_possession { get; set; }
        public DbSet<PriceHistoryDb> price_history { get; set; }
        public DbSet<StockDb> stock { get; set; }
        public DbSet<UsersDb> users { get; set; }
        public DbSet<UserFundsDb> user_funds { get; set; }
        public DbSet<TransactionHistoryDb> transaction_history { get; set; }
        public GenericDbContext(DbContextOptions<GenericDbContext> options) : base(options) 
        {
        }

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
}
