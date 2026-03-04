using ActualizeDataBaseWithRabbitMQ.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using RabbitMQAndGenericRepository.Repositorio;
using RabbitMQAndGenericRepository.Repositorio.DbEntities;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ActualizeDataBaseWithRabbitMQ.Infrastructure
{
    public class Idepomtecy
    {
        private readonly MessageRepository _messageRepository;
        public Idepomtecy(MessageRepository messageRepository)
        {
            _messageRepository = messageRepository;
        }
        public async Task ExecuteAsync<T>(string key, Func<Task<T>> func)
        {
            try
            {
                bool Processed = await TryReserveAsync(key);

                if (!Processed)
                {
                    Console.WriteLine("this message exist in the database and is being ignored");
                    return;
                }

                await func();
            }
            catch (Exception ex)
            {
                Console.WriteLine("execution failed with error: " + ex.Message);
            }
        }

        public async Task<bool> TryReserveAsync(string key)
        {
            try
            {
                await _messageRepository.AddAsync(new ProcessedMessages
                { 
                    message_Id = key,
                    procesedAt = DateTime.UtcNow
                });
            
                return true;
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine("execution failed with error: " + ex.Message);
                return false;
            }
        }
    }
}