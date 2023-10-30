using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using static ApiBinance.WebBinance;

namespace ApiBinance
{
    class Simulate_Trading
    {
        public static async Task<double> TESTGetAccountBalance() //Get info account Binance
        {
            long timestamp = (long)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds;
            string queryString = $"timestamp={timestamp}";
            string signature = BaseInfo.CalculateSignature(BaseInfo.TESTsecretKey, queryString);
            string endpoint = "/fapi/v2/account";
            string url = $"{BaseInfo.TESTBASEURL}{endpoint}?{queryString}&signature={signature}";
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("X-MBX-APIKEY", BaseInfo.TESTapiKey);
            HttpResponseMessage response = await client.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                var jsonResponse = await response.Content.ReadAsStringAsync();
                var accountInfo = JsonConvert.DeserializeObject<Models.FuturesAssetBalance>(jsonResponse);
                Console.WriteLine(jsonResponse);
                return accountInfo.totalMarginBalance;
            }
            else
            {
                throw new Exception("Ошибка запроса");
            }
        }
        public static async Task<Dictionary<string, double>> TESTGetOpenPositionFutures() //Информация о сделке(Профит по сделке)//Переделать, скорее всего нужно, чтобы данные выводились в массив.
        {
            long timestamp = (long)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds;
            string queryString = $"timestamp={timestamp}";
            string signature = BaseInfo.CalculateSignature(BaseInfo.TESTsecretKey, queryString);
            string endpoint = "/fapi/v2/positionRisk";
            string url = $"{BaseInfo.TESTBASEURL}{endpoint}?{queryString}&signature={signature}";

            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("X-MBX-APIKEY", BaseInfo.TESTapiKey);

            HttpResponseMessage response = await client.GetAsync(url);
            string responseBody = await response.Content.ReadAsStringAsync();
            List<Models.FuturesAssetBalance> positions = JsonConvert.DeserializeObject<List<Models.FuturesAssetBalance>>(responseBody);
            var test = new Dictionary<string, double>();
            foreach (Models.FuturesAssetBalance position in positions)
            {
                if (position.EntryPrice != 0)
                {
                    test.Add(position.symbol, position.unRealizedProfit);
                }
            }
            foreach (var open in test)
            {
                Console.WriteLine(open.Key + ": " + open.Value);
            }
            return test;
        }
        public static async Task OpenTPSL()
        {
            string symbol = "BTCUSDT"; // Например, для пары BTC/USDT

            // Данные о типе сделки, количестве и цене
            string side = "BUY"; // "BUY" или "SELL"
            decimal quantity = 0.001m; // Количество
            decimal price = 35000m; // Цена

            // Детали о stop loss и take profit
            decimal stopPrice = 34000; // Цена stop loss
            decimal takeProfitPrice = 36000; // Цена take profit

            // Формируем POST-запрос
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("https://testnet.binancefuture.com");
            client.DefaultRequestHeaders.Add("X-MBX-APIKEY", BaseInfo.TESTapiKey);

            string url = "/fapi/v1/order";
            string timestamp = DateTimeOffset.Now.ToUnixTimeMilliseconds().ToString();

            // Формируем параметры запроса
            Dictionary<string, string> parameters = new Dictionary<string, string>
            {
                { "symbol", symbol },
                { "side", side },
                { "type", "LIMIT" }, // Тип сделки - Limit (лимитная)
                { "quantity", quantity.ToString() },
                { "price", price.ToString() },
                { "timeInForce", "GTC" }, // Время действия - GTC (ждать и исполнять полностью или не исполнять)
                { "timestamp", timestamp },
                { "newOrderRespType", "RESULT" }, // Получить результат в ответе
                { "stopPrice", stopPrice.ToString() },
                { "stopLimitPrice", takeProfitPrice.ToString() },
                { "stopLimitTimeInForce", "GTC" }
            };

            // Сортируем параметры по ключам
            var sortedParams = string.Join("&", parameters.OrderBy(p => p.Key).Select(p => $"{p.Key}={p.Value}"));

            // Создаем подпись
            string signature = BaseInfo.CalculateSignature(BaseInfo.TESTsecretKey, sortedParams);

            // Формируем тело запроса
            var content = new FormUrlEncodedContent(parameters);

            // Добавляем подпись к параметрам
            url += "?" + sortedParams + "&signature=" + signature;

            // Отправляем запрос
            var response = await client.PostAsync(url, content);
            var responseContent = await response.Content.ReadAsStringAsync();

            // Обрабатываем ответ
            Console.WriteLine(responseContent);
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
            var payload = BaseInfo.CreateQueryString(parameters);
            var signature = BaseInfo.CalculateSignature(BaseInfo.TESTsecretKey, payload);

            // Добавление подписи и ключа API в заголовок запроса
            var requestUri = $"{BaseInfo.TESTBASEURL}{endpointPath}?{payload}&signature={signature}";
            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("X-MBX-APIKEY", BaseInfo.TESTapiKey);

            // Отправка POST запроса
            var response = await client.PostAsync(requestUri, null);

            // Получение ответа
            var content = await response.Content.ReadAsStringAsync();
            var orderResponse = JsonConvert.DeserializeObject<Models.FuturesAssetBalance>(content);
            var order = orderResponse.orderId;
            Console.WriteLine(order);
        }
        public static async Task PlaceBuyOrderStopMarket(string symbol, string side, string type, string quantity, string stopprice) //При достижении цены в 7100 активируется заявка на покупку по рыночной цене
        {
            string endpoint = "https://testnet.binancefuture.com/fapi/v1/order";
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
            var signature = BaseInfo.CalculateSignature(BaseInfo.TESTsecretKey, payload);
            // Добавление подписи и ключа API в заголовок запроса
            var requestUri = $"{endpoint}?{payload}&signature={signature}";
            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("X-MBX-APIKEY", BaseInfo.TESTapiKey);

            // Отправка POST запроса
            var response = await client.PostAsync(requestUri, null);

            // Получение ответа
            var content = await response.Content.ReadAsStringAsync();
            List<Models.FuturesAssetBalance> positions = JsonConvert.DeserializeObject<List<Models.FuturesAssetBalance>>(content);
            foreach (Models.FuturesAssetBalance position in positions)
            {
                //Console.WriteLine(position.orderId);
            }
            Console.WriteLine(content);
        }
    
        public static async Task EmergencyClosedPosition(string orderId)
        {
            HttpClient client = new HttpClient();
            string url = $"https://testnet.binancefuture.com/fapi/v1/order?orderId={orderId}";

            // Создание HTTP-заголовка для аутентификации
            var timestamp = (long)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalMilliseconds;
            var queryString = $"orderId={orderId}&timestamp={timestamp}";
            var signature = BaseInfo.CalculateSignature(BaseInfo.TESTsecretKey, queryString);

            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Add("X-MBX-APIKEY", BaseInfo.TESTapiKey);

            using var request = new HttpRequestMessage(new HttpMethod("DELETE"), url);
            request.Headers.Add("X-MBX-APIKEY", BaseInfo.TESTapiKey);
            request.Headers.Add("X-MBX-SIGNATURE", signature);
            request.Headers.Add("X-MBX-TIMESTAMP", timestamp.ToString());

            var response = await client.SendAsync(request);

            Console.WriteLine(await response.Content.ReadAsStringAsync());
        }
    }
}
