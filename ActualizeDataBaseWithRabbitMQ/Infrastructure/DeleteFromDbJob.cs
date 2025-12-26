using RabbitMQAndGenericRepository.Repositorio.DbEntities;

namespace ActualizeDataBaseWithRabbitMQ.Infrastructure
{
    public class DeleteFromDbJob
    {
        private readonly StocksAppUnitOfWork _stocksApp;
        private readonly PurchaseStocksUnitOfWork _purchase;
        private readonly SellStocksUnitOfWork _sell;
        private readonly LogInUnitOfWork _login;

        public DeleteFromDbJob(
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
            await _stocksApp.StockRepository.DeleteAsync(stock);
            await _purchase.StockRepository.DeleteAsync(stock);
            await _sell.StockRepository.DeleteAsync(stock);
        }
        public async Task Execute(PriceHistoryDb price)
        {
            await _stocksApp.PriceRepository.DeleteAsync(price);
            await _purchase.PriceRepository.DeleteAsync(price);
            await _sell.PriceRepository.DeleteAsync(price);
        }
        public async Task Execute(UsersDb user)
        {
            await _stocksApp.UserRepository.DeleteAsync(user);
            await _purchase.UserRepository.DeleteAsync(user);
            await _sell.UserRepository.DeleteAsync(user);
            await _login.UserRepository.DeleteAsync(user);
        }
        public async Task Execute(UserFundsDb userFunds)
        {
            await _stocksApp.UserFundsRepository.DeleteAsync(userFunds);
            await _login.UserFundsRepository.DeleteAsync(userFunds);
        } 
        public async Task Execute(InPossessionDb possession)
        {
            await _stocksApp.InPossessionRepository.DeleteAsync(possession);
        }
        public async Task Execute(TransactionHistoryDb transaction)
        {
            await _stocksApp.TransactionRepository.DeleteAsync(transaction);
        }
    }
}
