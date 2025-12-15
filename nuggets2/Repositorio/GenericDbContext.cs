using Microsoft.EntityFrameworkCore;
using RabbitMQAndGenericRepository.Repositorio.DbEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQAndGenericRepository.Repositorio
{
    public class GenericDbContext : DbContext
    {
        public DbSet<InPossessionDb> in_possession { get; set; }
        public DbSet<PriceDb> price { get; set; }
        public DbSet<StockDb> stock { get; set; }
        public DbSet<UsersDb> users { get; set; }
        public GenericDbContext(DbContextOptions<GenericDbContext> options) : base(options) 
        {

        }
    }
}
