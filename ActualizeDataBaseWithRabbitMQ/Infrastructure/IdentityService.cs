using ActualizeDataBaseWithRabbitMQ.Repositories;
using Microsoft.AspNetCore.Identity;
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
    public class IdentityService
    {
        public UserManager<IdentityUser> UserManager { get; }
        public UserFundsRepository UserFundsRepository { get; }
        public IdentityService(LogInDbContext logInDbContext, UserManager<IdentityUser> userManager, UserFundsRepository userFundsRepository)
        {
            UserManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            UserFundsRepository = new UserFundsRepository(logInDbContext);
        }
    }
}
