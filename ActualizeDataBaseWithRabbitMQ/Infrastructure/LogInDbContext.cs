using ActualizeDataBaseWithRabbitMQ.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ActualizeDataBaseWithRabbitMQ.Infrastructure
{
    public class LogInDbContext : DbContext
    {
        public DbSet<UsersDb> users { get; set; }
        public LogInDbContext(DbContextOptions<LogInDbContext> options) : base(options) {}
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<InPossessionDb>()
                .HasKey(p => new { p.owner_id, p.stock_id });  
        }
    }
}
