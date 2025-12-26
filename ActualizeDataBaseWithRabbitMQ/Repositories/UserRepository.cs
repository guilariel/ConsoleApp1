using RabbitMQAndGenericRepository.Repositorio.DbEntities;
using Microsoft.EntityFrameworkCore;
using RabbitMQAndGenericRepository.Repositorio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ActualizeDataBaseWithRabbitMQ.Repositories
{
    public interface IUserRepository : IRepository<UsersDb, int>
    {
        public Task<UsersDb> GetOneByNameAsync(string name);
    }
    public class UserRepository : EFRepository<UsersDb, int>, IUserRepository
    {
        public UserRepository(DbContext context) : base(context)
        {
        }
        public async Task<UsersDb> GetOneByNameAsync(string name)
        {
            UsersDb user = _entities.FirstOrDefault(u => u.name == name);
            return await Task.FromResult(user);
        }
    }
}
