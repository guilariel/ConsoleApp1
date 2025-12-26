using ActualizeDataBaseWithRabbitMQ.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using RabbitMQAndGenericRepository.Repositorio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ActualizeDataBaseWithRabbitMQ.Infrastructure
{
    public class LogInUnitOfWork: UnitOfWorkBase
    {
        public UserRepository UserRepository { get; }
        public UserFundsRepository UserFundsRepository { get; }
        public LogInUnitOfWork(IConfiguration config)
            : base(config.GetConnectionString("LogInDb"))
        {
            UserRepository = new UserRepository(_context);
            UserFundsRepository = new UserFundsRepository(_context);
        }
    }
}
