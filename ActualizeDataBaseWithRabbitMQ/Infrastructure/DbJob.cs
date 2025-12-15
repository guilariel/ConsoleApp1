using ActualizeDataBaseWithRabbitMQ.Domain.Entities;
using RabbitMQAndGenericRepository.RabbitMq;
using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace ActualizeDataBaseWithRabbitMQ.Infrastructure
{
    internal class DbJob
    {
        private readonly AddToDbJob _add;
        private readonly DeleteFromDbJob _delete;
        private readonly RabbitMessageService _rabbit;

        public DbJob(RabbitMessageService rabbit, AddToDbJob add, DeleteFromDbJob delete)
        {
            _rabbit = rabbit;
            _add = add;
            _delete = delete;
        }

        public async Task Execute()
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
                        // Puede ser UsersDb, PriceDb o InPossessionDb
                        if (DateTime.TryParse(data[1].ToString(), out DateTime date))
                        {
                            var price = new PriceDb
                            {
                                price = int.Parse(data[0].ToString()),
                                date = DateTime.SpecifyKind(date, DateTimeKind.Utc)
                            };
                            await Run(action, price);
                        }
                        break;
                    case 3:
                        if (!int.TryParse(data[0].ToString(), out _))
                        {
                            var user = new UsersDb
                            {
                                name = data[0].ToString(),
                                funds = int.Parse(data[1].ToString()),
                                password_hash = data[2].ToString()
                            };
                            await Run(action, user);
                        }
                        else
                        {
                            var poss = new InPossessionDb
                            {
                                owner_id = int.Parse(data[0].ToString()),
                                stock_id = int.Parse(data[1].ToString()),
                                amount = int.Parse(data[2].ToString())
                            };
                            await Run(action, poss);
                        }
                        break;
                    case 4:
                        // StockDb
                        var stock = new StockDb
                        {
                            symbol = data[0].ToString(),
                            name = data[1].ToString(),
                            description = data[2].ToString(),
                            price_id = int.Parse(data[3].ToString())
                        };
                        await Run(action, stock);
                        break;

                    default:
                        Console.WriteLine("Mensaje no reconocido: " + message);
                        break;
                }
            }
        }

        private Task Run(string action, object obj)
        {
            return action == "add"
                ? _callAdd(obj)
                : _callDelete(obj);
        }

        private Task _callAdd(object o) =>
            o switch
            {
                StockDb s => _add.Execute(s),
                PriceDb p => _add.Execute(p),
                UsersDb u => _add.Execute(u),
                InPossessionDb i => _add.Execute(i),
                _ => Task.CompletedTask
            };

        private Task _callDelete(object o) =>
            o switch
            {
                StockDb s => _delete.Execute(s),
                PriceDb p => _delete.Execute(p),
                UsersDb u => _delete.Execute(u),
                InPossessionDb i => _delete.Execute(i),
                _ => Task.CompletedTask
            };
    }
}
