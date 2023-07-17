using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net;
using System.Text;

namespace ApiBinance
{
    public class Transaction : BaseInfo
    {
        public class FuturesAccountInfo
        {
            [JsonProperty("assets")]
            public List<FuturesAssetBalance> Balances { get; init; }
        }

        public class FuturesAssetBalance
        {
            [JsonProperty("asset")]
            public string Asset { get; init; }

            [JsonProperty("availableBalance")]
            public double AvailableBalance { get; init; }
            [JsonProperty("symbol")]
            public string symbol { get; init; }
            [JsonProperty("positionAmt")]
            public decimal PositionAmt { get; init; }

            [JsonProperty("entryPrice")]
            public decimal EntryPrice { get; init; }

        }
        public static async Task GetOpenPositionFutures()
        {
            long timestamp = (long)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds;
            string queryString = $"timestamp={timestamp}";
            string signature = CalculateSignature(secretKey, queryString);
            string endpoint = "/fapi/v2/positionRisk";
            string url = $"{baseUrl}{endpoint}?{queryString}&signature={signature}";
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("X-MBX-APIKEY", apiKey);
            HttpResponseMessage response = await client.GetAsync(url);
            string responseBody = await response.Content.ReadAsStringAsync();
            List<FuturesAssetBalance> positions = JsonConvert.DeserializeObject<List<FuturesAssetBalance>>(responseBody);
            List<object> Open = new List<object>();
            foreach (FuturesAssetBalance position in positions)
            {
                if (position.EntryPrice != 0)
                {
                    
                    Console.WriteLine($"Символ: {position.symbol}, Позиция: {position.PositionAmt}, Средняя цена: {position.EntryPrice}" ); 
                }
            }
        }

        public static async Task<double> GetAccountInfo() //Get info account Binance
        {
            long timestamp = (long)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds;
            string queryString = $"timestamp={timestamp}";
            string signature = CalculateSignature(secretKey, queryString);
            string endpoint = "/fapi/v2/account";
            string url = $"{baseUrl}{endpoint}?{queryString}&signature={signature}";
            string asset = "USDT";
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("X-MBX-APIKEY", apiKey);

            HttpResponseMessage response = await client.GetAsync(url);
            string responseBody = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                var jsonResponse = await response.Content.ReadAsStringAsync();
                var accountInfo = JsonConvert.DeserializeObject<FuturesAccountInfo>(jsonResponse);
                var balance = accountInfo.Balances.FirstOrDefault(b => b.Asset == asset);
                Console.WriteLine(response.StatusCode);
                //Console.WriteLine(responseBody);
                return balance.AvailableBalance;
            }
            else
            {
                throw new Exception("Ошибка запроса");
            }
        }
        static string CreateQueryString(Dictionary<string, string> parameters)
        {
            var stringBuilder = new StringBuilder();
            foreach (var parameter in parameters)
            {
                stringBuilder.Append($"{parameter.Key}={parameter.Value}&");
            }
            var queryString = stringBuilder.ToString().TrimEnd('&');
            return queryString;
        }
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
            var payload = CreateQueryString(parameters);
            var signature = CalculateSignature(secretKey, payload);

            // Добавление подписи и ключа API в заголовок запроса
            var requestUri = $"{baseUrl}{endpointPath}?{payload}&signature={signature}";
            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("X-MBX-APIKEY", apiKey);

            // Отправка POST запроса
            var response = await client.PostAsync(requestUri, null);

            // Получение ответа
            var content = await response.Content.ReadAsStringAsync();
            //Console.WriteLine(content);
        }
    }
}
