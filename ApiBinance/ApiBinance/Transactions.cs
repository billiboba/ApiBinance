using Newtonsoft.Json;
using System.Text;

namespace ApiBinance.ApiBinance
{
    public class Transaction
    {

        public static async Task BuySell(string symbol, string side, string type, string quantity)
        {
            long timestamp = (long)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds;
            string endpointPath = "/fapi/v1/order";
            var parameters = new Dictionary<string, string>
        {
            { "symbol", symbol },  // Symbol of the futures contract
            { "side", side },        // SIDE: BUY or SELL
            { "type", type },     // TYPE: MARKET, LIMIT, STOP_MARKET etc.
            { "quantity", quantity },  // Quantity to buy or sell
            { "timestamp", timestamp.ToString() },  // Current timestamp
            };
            var payload = BaseInfo.CreateQueryString(parameters);
            var signature = BaseInfo.CalculateSignature(BaseInfo.secretKey, payload);

            // Добавление подписи и ключа API в заголовок запроса
            var requestUri = $"{BaseInfo.baseUrl}{endpointPath}?{payload}&signature={signature}";
            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("X-MBX-APIKEY", BaseInfo.apiKey);

            // Отправка POST запроса
            var response = await client.PostAsync(requestUri, null);

            // Получение ответа
            var content = await response.Content.ReadAsStringAsync();
            //Console.WriteLine(content);
        }

        public static async Task PlaceBuyOrderLimit(string symbol, string side, string type, string quantity, string stopprice)
        {
            string endpoint = "https://fapi.binance.com/fapi/v1/order";
            long timestamp = (long)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds;
            var parameters = new Dictionary<string, string>
        {
            { "symbol", symbol },  // Symbol of the futures contract
            { "side", side },        // SIDE: BUY or SELL
            { "type", type },     // TYPE: MARKET, LIMIT, STOP_MARKET etc.
            {"timeInForce", BaseInfo.time },
            { "quantity", quantity },  // Quantity to buy or sell\
            {"price",stopprice },
            { "timestamp", timestamp.ToString() },  // Current timestamp
            };
            var payload = BaseInfo.CreateQueryString(parameters);
            var signature = BaseInfo.CalculateSignature(BaseInfo.secretKey, payload);
            Console.WriteLine(payload);
            // Добавление подписи и ключа API в заголовок запроса
            var requestUri = $"{endpoint}?{payload}&signature={signature}";
            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("X-MBX-APIKEY", BaseInfo.apiKey);

            // Отправка POST запроса
            var response = await client.PostAsync(requestUri, null);

            // Получение ответа
            var content = await response.Content.ReadAsStringAsync();
            Console.WriteLine(content);
        }

        public static async Task PlaceBuyOrderStopMarket(string symbol, string side, string type, string quantity, string stopprice) //При достижении цены в 7100 активируется заявка на покупку по рыночной цене
        {
            string endpoint = "https://fapi.binance.com/fapi/v1/order";
            long timestamp = (long)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds;
            var parameters = new Dictionary<string, string>
        {
            { "symbol", symbol },  // Symbol of the futures contract
            { "side", side },        // SIDE: BUY or SELL
            { "type", type },     // TYPE: MARKET, LIMIT, STOP_MARKET etc.
            {"timeInForce", BaseInfo.time },
            { "quantity", quantity },  // Quantity to buy or sell\
            {"stopPrice",stopprice },
            { "timestamp", timestamp.ToString() },  // Current timestamp
            };
            var payload = BaseInfo.CreateQueryString(parameters);
            var signature = BaseInfo.CalculateSignature(BaseInfo.secretKey, payload);
            // Добавление подписи и ключа API в заголовок запроса
            var requestUri = $"{endpoint}?{payload}&signature={signature}";
            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("X-MBX-APIKEY", BaseInfo.apiKey);

            // Отправка POST запроса
            var response = await client.PostAsync(requestUri, null);

            // Получение ответа
            var content = await response.Content.ReadAsStringAsync();
            Console.WriteLine(content);
        }
    }
}
