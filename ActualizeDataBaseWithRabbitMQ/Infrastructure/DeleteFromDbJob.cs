using ActualizeDataBaseWithRabbitMQ.Domain.Entities;
using ActualizeDataBaseWithRabbitMQ.Infrastructure.SellStocksCruds;
using ActualizeDataBaseWithRabbitMQ.Infrastructure.StocksAppCruds;
using ActualizeDataBaseWithRabbitMQ.Infrastructure.PurchaseStocks;
using System.Threading.Tasks;
using ActualizeDataBaseWithRabbitMQ.Infrastructure.LogInCrud;

namespace ActualizeDataBaseWithRabbitMQ.Infrastructure
{
    public class DeleteFromDbJob
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

        public DeleteFromDbJob(
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
            Console.WriteLine($"Deleting stock: {stock.name} ({stock.symbol})");
            await _crudStocks.DeleteAsync(stock.id);
            await _purchaseCrudStocks.DeleteAsync(stock.id);
            await _stocksAppCrudStocks.DeleteAsync(stock.id);
        }

        public async Task Execute(PriceDb price)
        {
            Console.WriteLine($"Deleting price id {price.id}");
            await _crudPrices.DeleteOnePrice(price.id);
            await _purchaseCrudPrices.DeleteOnePrice(price.id);
            await _stocksAppCrudPrices.DeleteOnePrice(price.id);
        }

        public async Task Execute(UsersDb user)
        {
            Console.WriteLine($"Deleting user: {user.name}");
            await _crudUsers.DeleteAsync(user.id);
            await _purchaseCrudUsers.DeleteAsync(user.id);
            await _stocksAppCrudUsers.DeleteAsync(user.id);
            await _logInCrudUsers.DeleteAsync(user.id);
        }

        public async Task Execute(InPossessionDb possession)
        {
            Console.WriteLine($"Deleting possession: Owner {possession.owner_id} - Stock {possession.stock_id}");
            await _crudPossession.DeleteAsync(possession.owner_id, possession.stock_id,  possession.amount);
            await _purchaseCrudPossession.DeleteAsync(possession.owner_id, possession.stock_id,  possession.amount);
            await _stocksAppCrudPossession.DeleteAsync(possession.owner_id, possession.stock_id,  possession.amount);
        }
    }
}
