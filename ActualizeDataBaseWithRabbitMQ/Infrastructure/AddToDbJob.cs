using ActualizeDataBaseWithRabbitMQ.Domain.Entities;
using ActualizeDataBaseWithRabbitMQ.Infrastructure.LogInCrud;
using ActualizeDataBaseWithRabbitMQ.Infrastructure.PurchaseStocks;
using ActualizeDataBaseWithRabbitMQ.Infrastructure.SellStocksCruds;
using ActualizeDataBaseWithRabbitMQ.Infrastructure.StocksAppCruds;
using System.Threading.Tasks;

namespace ActualizeDataBaseWithRabbitMQ.Infrastructure
{
    public class AddToDbJob
    {
        private readonly SellStocksCrudPrices _crudPrices;
        private readonly SellStocksCrudStocks _crudStocks;
        private readonly SellStocksCrudUsers _crudUsers;
        private readonly SellStocksCrudPossession _crudPossession;

        private readonly PurchaseStocksCrudPrices _purchaseCrudPrices;
        private readonly PurchaseStocksCrudStocks _purchaseCrudStocks;
        private readonly PurchaseStocksCrudUsers _purchaseCrudUsers;
        private readonly PurchaseStocksCrudPossession _purchaseCrudPossession;

        private readonly StocksAppCrudPrices _stocksAppCrudPrices;
        private readonly StocksAppCrudStocks _stocksAppCrudStocks;
        private readonly StocksAppCrudUsers _stocksAppCrudUsers;
        private readonly StocksAppCrudPossession _stocksAppCrudPossession;

        private readonly LogInCrudUsers _logInCrudUsers;
        public AddToDbJob(
            SellStocksCrudPrices crudPrices, SellStocksCrudStocks crudStocks,
            SellStocksCrudUsers crudUsers, SellStocksCrudPossession crudPossession,
            PurchaseStocksCrudPrices purchaseCrudPrices, PurchaseStocksCrudStocks purchaseCrudStocks,
            PurchaseStocksCrudUsers purchaseCrudUsers, PurchaseStocksCrudPossession purchaseCrudPossession,
            StocksAppCrudPrices stocksAppCrudPrices, StocksAppCrudStocks stocksAppCrudStocks,
            StocksAppCrudUsers stocksAppCrudUsers, StocksAppCrudPossession stocksAppCrudPossession,
            LogInCrudUsers logInCrudUsers
            )
        {
            _crudPrices = crudPrices;
            _crudStocks = crudStocks;
            _crudUsers = crudUsers;
            _crudPossession = crudPossession;
            _purchaseCrudPrices = purchaseCrudPrices;
            _purchaseCrudStocks = purchaseCrudStocks;
            _purchaseCrudUsers = purchaseCrudUsers;
            _purchaseCrudPossession = purchaseCrudPossession;
            _stocksAppCrudPrices = stocksAppCrudPrices;
            _stocksAppCrudStocks = stocksAppCrudStocks;
            _stocksAppCrudUsers = stocksAppCrudUsers;
            _stocksAppCrudPossession = stocksAppCrudPossession;
            _logInCrudUsers = logInCrudUsers;
        }

        public async Task Execute(StockDb stock)
        {
            Console.WriteLine($"Adding stock: {stock.name} ({stock.symbol})");
            await _crudStocks.AddAsync(stock);
            await _purchaseCrudStocks.AddAsync(stock);
            await _stocksAppCrudStocks.AddAsync(stock);
        }

        public async Task Execute(PriceDb price)
        {
            Console.WriteLine($"Adding price: {price.price} at {price.date}");
            await _crudPrices.AddPrice(price);
            await _purchaseCrudPrices.AddPrice(price);
            await _stocksAppCrudPrices.AddPrice(price);
        }

        public async Task Execute(UsersDb user)
        {
            Console.WriteLine($"Adding user: {user.name} with funds {user.funds}");
            await _crudUsers.AddAsync(user);
            await _purchaseCrudUsers.AddAsync(user);
            await _stocksAppCrudUsers.AddAsync(user);
            await _logInCrudUsers.AddAsync(user);
        }

        public async Task Execute(InPossessionDb possession)
        {
            Console.WriteLine($"Adding possession: Owner {possession.owner_id} - Stock {possession.stock_id} Amount {possession.amount}");
            await _crudPossession.AddPossession(possession);
            await _purchaseCrudPossession.AddPossession(possession);
            await _stocksAppCrudPossession.AddPossession(possession);
        }
    }
}
