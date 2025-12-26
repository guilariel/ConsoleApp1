using RabbitMQAndGenericRepository.Repositorio.DbEntities;

namespace ActualizeDataBaseWithRabbitMQ.Infrastructure
{
    public class AddToDbJob
    {
        private readonly StocksAppUnitOfWork _stocksApp;
        private readonly PurchaseStocksUnitOfWork _purchase;
        private readonly SellStocksUnitOfWork _sell;
        private readonly LogInUnitOfWork _login;

        public AddToDbJob(
            StocksAppUnitOfWork stocksApp,
            PurchaseStocksUnitOfWork purchase,
            SellStocksUnitOfWork sell,
            LogInUnitOfWork login)
        {
            _stocksApp = stocksApp;
            _purchase = purchase;
            _sell = sell;
            _login = login;
        }

        public async Task Execute(StockDb stock)
        {
            await _stocksApp.StockRepository.AddAsync(stock);
            await _purchase.StockRepository.AddAsync(stock);
            await _sell.StockRepository.AddAsync(stock);
        }
        public async Task Execute(PriceHistoryDb price)
        {
            await _stocksApp.PriceRepository.AddAsync(price);
            await _purchase.PriceRepository.AddAsync(price);
            await _sell.PriceRepository.AddAsync(price);
        }
        public async Task Execute(UsersDb user)
        {
            await _stocksApp.UserRepository.AddAsync(user);
            await _purchase.UserRepository.AddAsync(user);
            await _sell.UserRepository.AddAsync(user);
            await _login.UserRepository.AddAsync(user);
        }
        public async Task Execute(UserFundsDb userFunds)
        {
            await _stocksApp.UserFundsRepository.AddAsync(userFunds);
            await _login.UserFundsRepository.AddAsync(userFunds);
        }
        public async Task Execute(InPossessionDb possession)
        {
            await _stocksApp.InPossessionRepository.AddAsync(possession);
        }
        public async Task Execute(TransactionHistoryDb transaction)
        {
            await _stocksApp.TransactionRepository.AddAsync(transaction);
        }
    }
}

/* public async Task Execute(StockDb stock)
        {
            await _stockRepository_StocksApp.AddAsync(stock);
            await _stockRepository_Purchase.AddAsync(stock);
            await _stockRepository_Sell.AddAsync(stock);
        }
        public async Task Execute(PriceDb price)
        {
            await _priceRepository_StocksApp.AddPrice(price);
            await _priceRepository_Purchase.AddPrice(price);
            await _priceRepository_Sell.AddPrice(price);
        }
        public async Task Execute(UsersDb user)
        {
            await _userRepository_StocksApp.AddAsync(user);
            await _userRepository_Purchase.AddAsync(user);
            await _userRepository_Sell.AddAsync(user);
            await _userRepository_LogIn.AddAsync(user);
        }
        public async Task Execute(UserFundsDb userFunds)
        {
            await userFundsRepository_StocksApp.AddAsync(userFunds);
            await _userFundsRepository_LogIn.AddAsync(userFunds);
        }
        public async Task Execute(InPossessionDb possession)
        {
            await _inPossessionRepository_StocksApp.AddPossession(possession);
        }
        public async Task Execute(TransactionHistoryDb transaction)
        {
            await _transactionRepositoryRepository_StocksApp.AddAsync(transaction);
        }*/