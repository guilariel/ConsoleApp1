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
            await _stocksApp.StockRepository.AddAsync(Clone(stock));
            await _purchase.StockRepository.AddAsync(Clone(stock));
            await _sell.StockRepository.AddAsync(Clone(stock));
        }

        public async Task Execute(PriceHistoryDb price)
        {
            await _stocksApp.PriceRepository.AddAsync(Clone(price));
            await _purchase.PriceRepository.AddAsync(Clone(price));
            await _sell.PriceRepository.AddAsync(Clone(price));
        }

        public async Task Execute(UsersDb user)
        {
            await _stocksApp.UserRepository.AddAsync(Clone(user));
            await _purchase.UserRepository.AddAsync(Clone(user));
            await _sell.UserRepository.AddAsync(Clone(user));
            await _login.UserRepository.AddAsync(Clone(user));
        }

        public async Task Execute(UserFundsDb funds)
        {
            await _stocksApp.UserFundsRepository.AddAsync(Clone(funds));
            await _login.UserFundsRepository.AddAsync(Clone(funds));
        }

        public async Task Execute(InPossessionDb possession)
        {
            await _stocksApp.InPossessionRepository.AddAsync(Clone(possession));
        }

        public async Task Execute(TransactionHistoryDb transaction)
        {
            await _stocksApp.TransactionRepository.AddAsync(Clone(transaction));
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
            stock_id  = p.stock_id,
            price = p.price,
            currency = p.currency,
            date = p.date
        };

        private static UsersDb Clone(UsersDb u) => new()
        {
            name = u.name,
            password_hash = u.password_hash
        };

        private static UserFundsDb Clone(UserFundsDb f) => new()
        {
            user_id = f.user_id,
            funds = f.funds,
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
            owner_id = t.owner_id,
            stock_id = t.stock_id,
            amount = t.amount,
            price = t.price,
            currency = t.currency,
            date = t.date,
            type = t.type
        };
    }
}
