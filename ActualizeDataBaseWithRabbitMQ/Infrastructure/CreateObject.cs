using Microsoft.AspNetCore.Identity;
using RabbitMQAndGenericRepository.Repositorio.DbEntities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ActualizeDataBaseWithRabbitMQ.Infrastructure
{
    public interface ICreateObject<T> where T : class
    {
        T ParseData(string[] data);
    }
    public class CreatePrice : ICreateObject<PriceHistoryDb>
    {
        public PriceHistoryDb ParseData(string[] data)
        {
            if (data.Length < 4)
                throw new ArgumentException("Invalid price data");

            if (!int.TryParse(data[0], out var stockId))
                throw new FormatException("Invalid stockId");

            if (!double.TryParse(data[1], out var price))
                throw new FormatException("Invalid price");

            if (!DateTime.TryParse(data[3], out var date))
                throw new FormatException("Invalid date");

            PriceHistoryDb priceHistory = new PriceHistoryDb
            {
                stock_id = stockId,
                price = price,
                currency = data[2],
                date = date
            };
            return priceHistory;
        }
    }
    public class CreateStock : ICreateObject<StockDb>
    {
        public StockDb ParseData(string[] data)
        {
            if (data.Length < 4)
                throw new ArgumentException("Invalid stock data");
            StockDb stock = new StockDb
            {
                id = data[0],
                symbol = data[1],
                name = data[2],
                description = data[3]
            };
            return stock;
        }
    }
    public class CreateInPossession : ICreateObject<InPossessionDb>
    {
        public InPossessionDb ParseData(string[] data)
        {
            if (data.Length < 3)
                throw new ArgumentException("Invalid in possession data");

            if (!int.TryParse(data[1], out var stockId))
                throw new FormatException("Invalid stockId");
            if (int.TryParse(data[2], out int amount))
                throw new FormatException("Invalid amount");

            InPossessionDb inPossession = new InPossessionDb
            {
                owner_id = data[0],
                stock_id = stockId,
                amount = amount
            };
            return inPossession;
        }
    }
    public class CreateTransactionHistory : ICreateObject<TransactionHistoryDb>
    {
        public TransactionHistoryDb ParseData(string[] data)
        {
            if (data.Length < 6)
                throw new ArgumentException("Invalid transaction history data");
            if (!int.TryParse(data[1], out int stockId))
                throw new FormatException("Invalid stockId");
            if (!int.TryParse(data[2], out int amount))
                throw new FormatException("Invalid amount");
            if (!double.TryParse(data[3], out double price))
                throw new FormatException("Invalid price");
            if (!DateTime.TryParse(data[5], out DateTime date))
                throw new FormatException("Invalid date");
            TransactionHistoryDb transaction = new TransactionHistoryDb
            {
                owner_id = data[0],
                stock_id = stockId,
                amount = amount,
                price = price,
                currency = data[4],
                date = date
            };
            return transaction;
        }
    }
    public class CreateUserFunds : ICreateObject<UserFundsDb>
    {
        public UserFundsDb ParseData(string[] data)
        {
            if (data.Length < 3)
                throw new ArgumentException("Invalid user funds data");
            if (!int.TryParse(data[1], out int funds))
                throw new FormatException("Invalid funds");

            UserFundsDb userFunds = new UserFundsDb
            {
                user_id = data[0],
                funds = funds,
                currency = data[2]
            };
            return userFunds;
        }
    }
    public class CreateUser : ICreateObject<UsersDb>
    {
        public UsersDb ParseData(string[] data)
        {
            if (data.Length < 2)
                throw new ArgumentException("Invalid user data");

            UsersDb user = new UsersDb
            {
                id = data[0],
                name = data[1],
            };
            return user;
        }
    }
    public class CreateIdentityUser : ICreateObject<IdentityUser>
    {
        public IdentityUser ParseData(string[] data)
        {
            if(data.Length < 1)
                throw new ArgumentException("Invalid identity user data");
            //llenar todos los datos y si no existen poner vacios o null, dependiendo del tipo de dato, para no tener problemas al crear el objeto
            IdentityUser user = new IdentityUser
            {
                UserName = data.Length > 0 ? data[0] : string.Empty,
                Email = data.Length > 1 ? data[1] : string.Empty,
                PasswordHash = data.Length > 2 ? data[2] : string.Empty,
                SecurityStamp = data.Length > 3 ? data[3] : string.Empty,
                ConcurrencyStamp = data.Length > 4 ? data[4] : string.Empty,
                PhoneNumber = data.Length > 5 ? data[5] : string.Empty,
                TwoFactorEnabled = data.Length > 6 && bool.TryParse(data[6], out var twoFactor) ? twoFactor : false,
                LockoutEnabled = data.Length > 7 && bool.TryParse(data[7], out var lockout) ? lockout : false,
                AccessFailedCount = data.Length > 8 && int.TryParse(data[8], out var accessFailed) ? accessFailed : 0
            };
            return user;
        }
    }
    public interface IObjectCreatorResolver
    {
        object Create(string type, string[] data);
    }

    public class ObjectCreatorResolver : IObjectCreatorResolver
    {
        private readonly Dictionary<string, object> _creators;
        public ObjectCreatorResolver()
        {
            _creators = new Dictionary<string, object>
            {
                { "PriceHistory", new CreatePrice() },
                { "Stock", new CreateStock() },
                { "InPossession", new CreateInPossession() },
                { "TransactionHistory", new CreateTransactionHistory() },
                { "UserFunds", new CreateUserFunds() },
                { "User", new CreateUser() },
                { "IdentityUser", new CreateIdentityUser() },
            };
        }
        public object Create(string type, string[] data)
        {
            if (_creators.TryGetValue(type, out var creator))
            {
                var method = creator.GetType().GetMethod("ParseData");
                return method.Invoke(creator, new object[] { data });
            }
            throw new ArgumentException($"No creator found for type: {type}");
        }

    }
}