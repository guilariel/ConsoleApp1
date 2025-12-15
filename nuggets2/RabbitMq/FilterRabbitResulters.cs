using RabbitMQAndGenericRepository.Repositorio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace RabbitMQAndGenericRepository.RabbitMq
{
    public class FilterRabbitResultes
    {
        public async Task<List<T>> FilterResults<T>(List<string> data) where T : class, new()
        {
            List<T> elements = new List<T>();
            Type type = typeof(T);
            var props = type.GetProperties(); // Usaremos las propiedades en orden

            foreach (var item in data)
            {
                // Parseamos como ARRAY
                JsonDocument doc = JsonDocument.Parse(item);
                JsonElement root = doc.RootElement;

                if (root.ValueKind != JsonValueKind.Array)
                    continue;

                // Acción = primer elemento (índice 0)
                string action = root[0].GetString();

                // Creamos objeto
                var element = new T();

                // Comenzamos desde el índice 1 porque el 0 es "add" o "sell"
                for (int i = 1; i < root.GetArrayLength() && i <= props.Length; i++)
                {
                    var prop = props[i - 1];
                    object value = ConvertToType(root[i], prop.PropertyType);
                    prop.SetValue(element, value);
                }

                elements.Add(element);
            }

            return elements;
        }
        private object ConvertToType(JsonElement json, Type targetType)
        {
            // string
            if (targetType == typeof(string))
                return json.GetString();

            // int, decimal, double, long...
            if (targetType.IsPrimitive || targetType == typeof(decimal))
                return Convert.ChangeType(json.GetString(), targetType);

            // DateTime
            if (targetType == typeof(DateTime))
                return DateTime.Parse(json.GetString());

            return null;
        }
    }
}