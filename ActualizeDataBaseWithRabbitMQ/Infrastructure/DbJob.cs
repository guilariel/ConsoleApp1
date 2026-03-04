using Microsoft.AspNetCore.Identity;
using RabbitMQAndGenericRepository.RabbitMq;
using RabbitMQAndGenericRepository.Repositorio.DbEntities;
using System.Globalization;
using System.Text.Json;
namespace ActualizeDataBaseWithRabbitMQ.Infrastructure
{
    internal class DbJob
    {
        private readonly ICommandResolver _command;
        private readonly IObjectCreatorResolver _objectCreator;
        private readonly Idepomtecy _idepomtecy;
        private readonly RabbitMessageService _rabbit;
        public DbJob(RabbitMessageService rabbit, ICommandResolver command, Idepomtecy idepomtecy,IObjectCreatorResolver objectCreator)
        {
            _rabbit = rabbit;
            _command = command;
            _idepomtecy = idepomtecy;
            _objectCreator = objectCreator;
        }

        public async Task Execute()
        {
            var rawMessages = await _rabbit.GetRawMessagesAsync();
        
            foreach (string message in rawMessages)
            {
                var root = JsonDocument.Parse(message).RootElement;
                if (root.ValueKind != JsonValueKind.Array) continue;

                string action = root[0].GetString();
                string type = root[1].GetString();
                string messageId = root[2].GetString();
                int length = root.GetArrayLength(); // sin la acción

                var data = root.EnumerateArray().Skip(3).ToArray();
                string[] dataStrings = data.Select(d => d.GetString()).ToArray();
                object obj = _objectCreator.Create(type, dataStrings);

                var resolveMethod = _command.GetType().GetMethod("Resolve");
                var genericResolve = resolveMethod!.MakeGenericMethod(obj.GetType());
                var commandInstance = genericResolve.Invoke(_command, new object[] { action });
                var executeMethod = commandInstance!.GetType().GetMethod("Execute");

                await _idepomtecy.ExecuteAsync<object>(messageId, async () =>
                {
                    await (Task)executeMethod.Invoke(commandInstance, new object[] { obj })!;
                    return null;
                });
            }
        }
    }
}









//private Task Run(string action, object obj)
//{
//    return action == "add"
//        ? _callAdd(obj)
//        : _callDelete(obj);
//}

//private Task _callAdd(object o) =>
//    o switch
//    {
//        StockDb s => _add.Execute(s),
//        PriceHistoryDb p => _add.Execute(p),
//        UsersDb u => _add.Execute(u),
//        InPossessionDb i => _add.Execute(i),
//        UserFundsDb f => _add.Execute(f),
//        TransactionHistoryDb t => _add.Execute(t),
//        IdentityUser identityUser => _add.Execute(identityUser),
//        _ => Task.CompletedTask
//    };

//private Task _callDelete(object o) =>
//    o switch
//    {
//        StockDb s => _delete.Execute(s),
//        PriceHistoryDb p => _delete.Execute(p),
//        UsersDb u => _delete.Execute(u),
//        InPossessionDb i => _delete.Execute(i),
//        UserFundsDb f => _delete.Execute(f),
//        TransactionHistoryDb t => _delete.Execute(t),
//        IdentityUser identityUser => _delete.Execute(identityUser),
//        _ => Task.CompletedTask
//    };

































/*     public async Task Execute()
        {
            var rawMessages = await _rabbit.GetRawMessagesAsync();

            foreach (string message in rawMessages)
            {
                var root = JsonDocument.Parse(message).RootElement;
                if (root.ValueKind != JsonValueKind.Array) continue;

                string action = root[0].GetString();
                int length = root.GetArrayLength() - 1; // sin la acción

                // QUITAMOS EL "add"/"sell" y trabajamos solo con los datos
                var data = root.EnumerateArray().Skip(1).ToArray();

                switch (length)
                {
                    case 2:
                        UsersDb usersDb = new UsersDb
                        {
                            name = data[0].GetString(),
                            password_hash = data[1].GetString()
                        };
                        await Run(action, usersDb);
                        break;
                    case 3:
                        if (int.TryParse(data[0].GetString(), out _) && int.TryParse(data[1].GetString(), out _) && int.TryParse(data[2].GetString(), out _))
                        {
                            InPossessionDb inPossessionDb = new InPossessionDb
                            {
                                owner_id = int.Parse(data[0].GetString()),
                                stock_id = int.Parse(data[1].GetString()),
                                amount = int.Parse(data[2].GetString())
                            };
                            await Run(action, inPossessionDb);
                        }
                        else if( double.TryParse(data[0].GetString(), out _))
                        {
                            UserFundsDb userFundsDb = new UserFundsDb
                            {
                                user_id = int.Parse(data[0].GetString()),
                                funds = double.Parse(data[1].GetString()),
                                currency = data[2].GetString()
                            };
                            await Run(action, userFundsDb);
                        }
                        else
                        {
                            StockDb stockDb = new StockDb
                            {
                                symbol = data[0].GetString(),
                                name = data[1].GetString(),
                                description = data[2].GetString()
                            };
                            await Run(action, stockDb);
                        }
                        break;
                    case 4:
                        PriceHistoryDb priceHistoryDb = new PriceHistoryDb
                        {
                            stock_id = int.Parse(data[0].GetString()),
                            price = double.Parse(data[1].GetString()),
                            currency = data[2].GetString(),
                            date = DateTime.Parse(
                                 data[3].GetString(),
                                 CultureInfo.InvariantCulture,
                                 DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal
                            )
                        };
                        await Run(action, priceHistoryDb);
                        break;
                    case 7:
                        TransactionHistoryDb transactionHistoryDb = new TransactionHistoryDb
                        {
                            owner_id = int.Parse(data[0].GetString()),
                            stock_id = int.Parse(data[1].GetString()),
                            amount = int.Parse(data[2].GetString()),
                            price = double.Parse(data[3].GetString()),
                            currency = data[4].GetString(),
                            date = DateTime.ParseExact(
    data[5].GetString(),
    "dd/MM/yyyy HH:mm:ss",
    CultureInfo.InvariantCulture,
    DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal
),

                            type = data[6].GetString()
                        };
                        await Run(action, transactionHistoryDb);
                        break;
                    //em caso de que el mensaje sea identityUser
                    case 11:
                        IdentityUser identityUser = new IdentityUser
                        {
                            Id = data[0].GetString(),
                            UserName = data[1].GetString(),
                            NormalizedUserName = data[2].GetString(),
                            Email = data[3].GetString(),
                            NormalizedEmail = data[4].GetString(),
                            EmailConfirmed = bool.Parse(data[5].GetString()),
                            PasswordHash = data[6].GetString(),
                            SecurityStamp = data[7].GetString(),
                            ConcurrencyStamp = data[8].GetString(),
                            PhoneNumber = data[9].GetString(),
                            PhoneNumberConfirmed = bool.Parse(data[10].GetString())
                        };
                      await Run(action, identityUser);
                      break;
                    default:
                           Console.WriteLine("Mensaje no reconocido: " + message);
                            break;
                }
            }
        }*/