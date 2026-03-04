using Microsoft.AspNetCore.Identity;
using RabbitMQAndGenericRepository.Repositorio.DbEntities;


namespace ActualizeDataBaseWithRabbitMQ.Infrastructure
{
    public class AddStockCommand : ICommand<StockDb>
    {
        private readonly StocksAppUnitOfWork _stocksApp;
        private readonly PurchaseStocksUnitOfWork _purchase;
        private readonly SellStocksUnitOfWork _sell;

        public AddStockCommand(
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
            await _stocksApp.StockRepository.AddAsync(Clone(stock));
            await _purchase.StockRepository.AddAsync(Clone(stock));
            await _sell.StockRepository.AddAsync(Clone(stock));
        }
        private static StockDb Clone(StockDb s) => new()
        {
            symbol = s.symbol,
            name = s.name,
            description = s.description
        };
    }
    public class AddPriceCommand : ICommand<PriceHistoryDb>
    {
        private readonly StocksAppUnitOfWork _stocksApp;
        private readonly PurchaseStocksUnitOfWork _purchase;
        private readonly SellStocksUnitOfWork _sell;

        public AddPriceCommand(
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
            await _stocksApp.PriceRepository.AddAsync(Clone(price));
            await _purchase.PriceRepository.AddAsync(Clone(price));
            await _sell.PriceRepository.AddAsync(Clone(price));
        }
        private static PriceHistoryDb Clone(PriceHistoryDb p) => new()
        {
            stock_id = p.stock_id,
            price = p.price,
            currency = p.currency,
            date = p.date
        };
    }
    public class AddUserCommand : ICommand<UsersDb>
    {
        private readonly StocksAppUnitOfWork _stocksApp;
        private readonly PurchaseStocksUnitOfWork _purchase;
        private readonly SellStocksUnitOfWork _sell;
        private readonly IdentityService _identityService;

        public AddUserCommand(
            StocksAppUnitOfWork stocksApp,
            PurchaseStocksUnitOfWork purchase,
            SellStocksUnitOfWork sell)
        {
            _stocksApp = stocksApp;
            _purchase = purchase;
            _sell = sell;
        }
        public async Task Execute(UsersDb user)
        {
            await _stocksApp.UserRepository.AddAsync(Clone(user));
            await _purchase.UserRepository.AddAsync(Clone(user));
            await _sell.UserRepository.AddAsync(Clone(user));
        }
        private static UsersDb Clone(UsersDb u) => new()
        {
            name = u.name
        };
    }
    public class AddUserFundsCommand : ICommand<UserFundsDb>
    {
        private readonly StocksAppUnitOfWork _stocksApp;
        private readonly IdentityService _identityService;
        public AddUserFundsCommand(
            StocksAppUnitOfWork stocksAppUnitOfWork,
            IdentityService identityService)
        {
            _stocksApp = stocksAppUnitOfWork;
            _identityService = identityService;
        }
        public async Task Execute(UserFundsDb funds)
        {
            await _stocksApp.UserFundsRepository.AddAsync(Clone(funds));
            await _identityService.UserFundsRepository.AddAsync(Clone(funds));
        }
        private static UserFundsDb Clone(UserFundsDb f) => new()
        {
            user_id = f.user_id,
            funds = f.funds,
            currency = f.currency
        };
    }
    public class AddInPossessionCommand : ICommand<InPossessionDb>
    {
        private readonly StocksAppUnitOfWork _stocksApp;
        public AddInPossessionCommand(StocksAppUnitOfWork stocksAppUnitOfWork)
        {
            _stocksApp = stocksAppUnitOfWork;
        }
        public async Task Execute(InPossessionDb possession)
        {
            await _stocksApp.InPossessionRepository.AddAsync(possession);
        }
    }
    public class AddTransactionCommand : ICommand<TransactionHistoryDb>
    {
        private readonly StocksAppUnitOfWork _stocksApp;
        public AddTransactionCommand(StocksAppUnitOfWork stocksAppUnitOfWork)
        {
            _stocksApp = stocksAppUnitOfWork;
        }
        public async Task Execute(TransactionHistoryDb transaction)
        {
            await _stocksApp.TransactionRepository.AddAsync(transaction);
        }
    }
    public class AddIdentityUserCommand : ICommand<IdentityUser>
    {
        private readonly IdentityService _identityService;
        public AddIdentityUserCommand(IdentityService identityService)
        {
            _identityService = identityService;
        }
        public async Task Execute(IdentityUser user)
        {
            await _identityService.UserManager.CreateAsync(user);
        }
    }
}


