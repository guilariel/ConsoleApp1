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

namespace ActualizeDataBaseWithRabbitMQ.Repositories
{
    public interface IUserFundsRepository : IRepository<UserFundsDb>
    {
        public Task AddFundsAsync(int id, string currency, double cost);
        public Task SellFundsAsync(int id, string currency, double cost);
    }
    public class UserFundsRepository : EFRepository<UserFundsDb>
    {
        public UserFundsRepository(DbContext context) : base(context)
        {
        }
        public UserFundsRepository(IdentityDbContext<IdentityUser> context) : base(context)
        {
        }
        public async Task AddFundsAsync(string id, string currency, double cost)
        {
            UserFundsDb? user_funds = _entities.FirstOrDefault(u => u.user_id == id && u.currency == currency);
            user_funds.funds += cost;
            await Task.CompletedTask;
        }
        public async Task SellFundsAsync(string id, string currency, double cost)
        {
            UserFundsDb? user_funds = _entities.FirstOrDefault(u => u.user_id == id && u.currency == currency);
            user_funds.funds -= cost;
            await Task.CompletedTask;
        }
    }
}
