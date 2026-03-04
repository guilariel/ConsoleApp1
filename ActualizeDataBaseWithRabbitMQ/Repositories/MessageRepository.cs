using ActualizeDataBaseWithRabbitMQ.Infrastructure;
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
    public interface IMessageRepository : IRepository<ProcessedMessages>
    {
    }
    public class MessageRepository : EFRepository<ProcessedMessages>, IMessageRepository
    {
        public MessageRepository(StocksAppDbContext context) : base(context)
        {
        }
    }
}
