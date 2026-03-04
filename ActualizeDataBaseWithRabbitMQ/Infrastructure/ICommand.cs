using Microsoft.Extensions.DependencyInjection;
using RabbitMQAndGenericRepository.Repositorio.DbEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ActualizeDataBaseWithRabbitMQ.Infrastructure
{ 
    public interface ICommand<T> where T : class
    {
        Task Execute(T type);
    }
    public class ExecuteCommand
    {
        public async Task DispatchCommand<T>(ICommand<T> command, T data) where T : class 
        {
            await command.Execute(data);
        }
    }
    public interface ICommandResolver
    {
        ICommand<T> Resolve<T>(string type) where T : class;
    }
    public class CommandResolver : ICommandResolver
    {
        private readonly IServiceProvider _serviceProvider;
        public CommandResolver(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
        public ICommand<T> Resolve<T>(string type) where T : class
        {
            return type switch
            {
                "AddStock" => (ICommand<T>)_serviceProvider.GetRequiredService<AddStockCommand>(),
                "AddPrice" => (ICommand<T>)_serviceProvider.GetRequiredService<AddPriceCommand>(),
                "AddTransaction" => (ICommand<T>)_serviceProvider.GetRequiredService<AddTransactionCommand>(),
                "AddFunds" => (ICommand<T>)_serviceProvider.GetRequiredService<AddUserFundsCommand>(),
                "AddUser" => (ICommand<T>)_serviceProvider.GetRequiredService<AddUserCommand>(),
                "AddPossession" => (ICommand<T>)_serviceProvider.GetRequiredService<AddInPossessionCommand>(),
                "AddIdentityUser" => (ICommand<T>)_serviceProvider.GetRequiredService<AddIdentityUserCommand>(),
                "DeleteStock" => (ICommand<T>)_serviceProvider.GetRequiredService<DeleteStockCommand>(),
                "DeletePrice" => (ICommand<T>)_serviceProvider.GetRequiredService<DeletePriceCommand>(),
                "DeleteTransaction" => (ICommand<T>)_serviceProvider.GetRequiredService<DeleteTransactionCommand>(),
                "DeleteFunds" => (ICommand<T>)_serviceProvider.GetRequiredService<DeleteUserFundsCommand>(),
                "DeleteUser" => (ICommand<T>)_serviceProvider.GetRequiredService<DeleteUserCommand>(),
                "DeletePossession" => (ICommand<T>)_serviceProvider.GetRequiredService<DeleteInPossessionCommand>(),
                "DeleteIdentityUser" => (ICommand<T>)_serviceProvider.GetRequiredService<DeleteIdentityUserCommand>(),
                _ => throw new Exception("Command not found")
            };
        }
    }
}
