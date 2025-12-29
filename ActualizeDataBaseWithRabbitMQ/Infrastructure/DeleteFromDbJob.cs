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
            await _stocksApp.StockRepository.DeleteAsync(Clone(stock));
            await _purchase.StockRepository.DeleteAsync(Clone(stock));
            await _sell.StockRepository.DeleteAsync(Clone(stock));
        }

        public async Task Execute(PriceHistoryDb price)
        {
            await _stocksApp.PriceRepository.DeleteAsync(Clone(price));
            await _purchase.PriceRepository.DeleteAsync(Clone(price));
            await _sell.PriceRepository.DeleteAsync(Clone(price));
        }

        public async Task Execute(UsersDb user)
        {
            await _stocksApp.UserRepository.DeleteAsync(Clone(user));
            await _purchase.UserRepository.DeleteAsync(Clone(user));
            await _sell.UserRepository.DeleteAsync(Clone(user));
            await _login.UserRepository.DeleteAsync(Clone(user));
        }

        public async Task Execute(UserFundsDb funds)
        {
            await _stocksApp.UserFundsRepository.DeleteAsync(Clone(funds));
            await _login.UserFundsRepository.DeleteAsync(Clone(funds));
        }

        public async Task Execute(InPossessionDb possession)
        {
            await _stocksApp.InPossessionRepository.DeleteAsync(Clone(possession));
        }

        public async Task Execute(TransactionHistoryDb transaction)
        {
            await _stocksApp.TransactionRepository.DeleteAsync(Clone(transaction));
        }

        // ---------- CLONES ----------

        private static StockDb Clone(StockDb s) => new()
        {
            symbol = s.symbol,
            name = s.name,
            description = s.description
        };

        private static PriceHistoryDb Clone(PriceHistoryDb p) => new()
        {
            stock_id = p.stock_id
        };

        private static UsersDb Clone(UsersDb u) => new()
        {
            id = u.id
        };

        private static UserFundsDb Clone(UserFundsDb f) => new()
        {
            user_id = f.user_id,
            currency = f.currency
        };

        private static InPossessionDb Clone(InPossessionDb i) => new()
        {
            owner_id = i.owner_id,
            stock_id = i.stock_id,
            amount = i.amount
        };

        private static TransactionHistoryDb Clone(TransactionHistoryDb t) => new()
        {
            id = t.id
        };
    }
}
