using Microsoft.AspNetCore.Identity;
using RabbitMQAndGenericRepository.Repositorio.DbEntities;

namespace ActualizeDataBaseWithRabbitMQ.Infrastructure
{
    public class DeleteStockCommand : ICommand<StockDb>
    {
        private readonly StocksAppUnitOfWork _stocksApp;
        private readonly PurchaseStocksUnitOfWork _purchase;
        private readonly SellStocksUnitOfWork _sell;

        public DeleteStockCommand(
            StocksAppUnitOfWork stocksApp,
            PurchaseStocksUnitOfWork purchase,
            SellStocksUnitOfWork sell)
        {
            _stocksApp = stocksApp;
            _purchase = purchase;
            _sell = sell;
        }

        public async Task Execute(StockDb stock)
        {
            await _stocksApp.StockRepository.DeleteAsync(Clone(stock));
            await _purchase.StockRepository.DeleteAsync(Clone(stock));
            await _sell.StockRepository.DeleteAsync(Clone(stock));
        }

        private static StockDb Clone(StockDb s) => new()
        {
            id = s.id,
            name = s.name,
            symbol = s.symbol,
            description = s.description
        };
    }

    public class DeletePriceCommand : ICommand<PriceHistoryDb>
    {
        private readonly StocksAppUnitOfWork _stocksApp;
        private readonly PurchaseStocksUnitOfWork _purchase;
        private readonly SellStocksUnitOfWork _sell;

        public DeletePriceCommand(
            StocksAppUnitOfWork stocksApp,
            PurchaseStocksUnitOfWork purchase,
            SellStocksUnitOfWork sell)
        {
            _stocksApp = stocksApp;
            _purchase = purchase;
            _sell = sell;
        }

        public async Task Execute(PriceHistoryDb price)
        {
            await _stocksApp.PriceRepository.DeleteAsync(Clone(price));
            await _purchase.PriceRepository.DeleteAsync(Clone(price));
            await _sell.PriceRepository.DeleteAsync(Clone(price));
        }

        private static PriceHistoryDb Clone(PriceHistoryDb p) => new()
        {
            stock_id = p.stock_id,
            price = p.price,
            currency = p.currency,
            date = p.date
        };
    }

    public class DeleteUserCommand : ICommand<UsersDb>
    {
        private readonly StocksAppUnitOfWork _stocksApp;
        private readonly PurchaseStocksUnitOfWork _purchase;
        private readonly SellStocksUnitOfWork _sell;
        private readonly IdentityService _identityService;

        public DeleteUserCommand(
            StocksAppUnitOfWork stocksApp,
            PurchaseStocksUnitOfWork purchase,
            SellStocksUnitOfWork sell,
            IdentityService identityService)
        {
            _stocksApp = stocksApp;
            _purchase = purchase;
            _sell = sell;
            _identityService = identityService;
        }

        public async Task Execute(UsersDb user)
        {
            await _stocksApp.UserRepository.DeleteAsync(Clone(user));
            await _purchase.UserRepository.DeleteAsync(Clone(user));
            await _sell.UserRepository.DeleteAsync(Clone(user));

            var identityUser = new IdentityUser { Id = user.id.ToString() };
            await _identityService.UserManager.DeleteAsync(identityUser);
        }

        private static UsersDb Clone(UsersDb u) => new()
        {
            name = u.name
        };

    }

    public class DeleteUserFundsCommand : ICommand<UserFundsDb>
    {
        private readonly StocksAppUnitOfWork _stocksApp;
        private readonly IdentityService _identityService;

        public DeleteUserFundsCommand(
            StocksAppUnitOfWork stocksAppUnitOfWork,
            IdentityService identityService)
        {
            _stocksApp = stocksAppUnitOfWork;
            _identityService = identityService;
        }

        public async Task Execute(UserFundsDb funds)
        {
            await _stocksApp.UserFundsRepository.DeleteAsync(Clone(funds));
            await _identityService.UserFundsRepository.DeleteAsync(Clone(funds));
        }

        private static UserFundsDb Clone(UserFundsDb f) => new()
        {
            user_id = f.user_id,
            funds = f.funds,
            currency = f.currency
        };
    }

    public class DeleteInPossessionCommand : ICommand<InPossessionDb>
    {
        private readonly StocksAppUnitOfWork _stocksApp;

        public DeleteInPossessionCommand(StocksAppUnitOfWork stocksAppUnitOfWork)
        {
            _stocksApp = stocksAppUnitOfWork;
        }

        public async Task Execute(InPossessionDb possession)
        {
            await _stocksApp.InPossessionRepository.DeleteAsync(possession);
        }
    }

    public class DeleteTransactionCommand : ICommand<TransactionHistoryDb>
    {
        private readonly StocksAppUnitOfWork _stocksApp;

        public DeleteTransactionCommand(StocksAppUnitOfWork stocksAppUnitOfWork)
        {
            _stocksApp = stocksAppUnitOfWork;
        }

        public async Task Execute(TransactionHistoryDb transaction)
        {
            await _stocksApp.TransactionRepository.DeleteAsync(transaction);
        }
    }
    public class DeleteIdentityUserCommand : ICommand<IdentityUser>
    {
        private readonly IdentityService _identityService;
        public DeleteIdentityUserCommand(IdentityService identityService)
        {
            _identityService = identityService;
        }
        public async Task Execute(IdentityUser user)
        {
            await _identityService.UserManager.DeleteAsync(user);
        }
    }
}