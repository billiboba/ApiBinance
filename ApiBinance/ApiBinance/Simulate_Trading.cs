using Nancy;
using Nancy.Responses;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Reflection.Metadata;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using static ApiBinance.ApiBinance.WebBinance;
using static System.Net.Mime.MediaTypeNames;

namespace ApiBinance.ApiBinance
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
        public static async Task<List<string>> TESTGetOpenPositionFutures() //Информация о сделке(Профит по сделке)//Переделать, скорее всего нужно, чтобы данные выводились в массив.
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
            List<Models.FuturesAssetBalance> orderResponse = JsonConvert.DeserializeObject<List<Models.FuturesAssetBalance>>(responseBody);
            List<string> opens = new List<string>();
            foreach (var test in orderResponse)
            {
                if (test.EntryPrice != 0)
                {
                    opens.Add(test.symbol);
                }
            }

            return opens;
        }
        public static async Task<double> BuySell(string symbol, string side, string type, double quantity)
        {
            long timestamp = (long)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds;
            string endpointPath = "/fapi/v1/order";
            var parameters = new Dictionary<string, string>
        {
            { "symbol", symbol },  // Symbol of the futures contract
            { "side", side },        // SIDE: BUY or SELL
            { "type", type },     // TYPE: MARKET, LIMIT, STOP_MARKET etc.
            { "quantity", quantity.ToString().Replace(",",".") },  // Quantity to buy or sell
            { "timestamp", timestamp.ToString() },  // Current timestamp
            };
            var payload = BaseInfo.CreateQueryString(parameters);
            var signature = BaseInfo.CalculateSignature(BaseInfo.TESTsecretKey, payload);

            var requestUri = $"{BaseInfo.TESTBASEURL}{endpointPath}?{payload}&signature={signature}";
            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("X-MBX-APIKEY", BaseInfo.TESTapiKey);

            var response = await client.PostAsync(requestUri, null);
            var content = await response.Content.ReadAsStringAsync();
            var orderResponse = JsonConvert.DeserializeObject<Models.FuturesAssetBalance>(content);
            var order = orderResponse.orderId;
            Console.WriteLine(content);
            Console.WriteLine(orderResponse.symbol + "is Opened");
            return order;
        }
        public static async Task<double> PlaceBuyOrderStopMarket(string symbol, string side, string type, double quantity, double stopprice) //При достижении цены в 7100 активируется заявка на покупку по рыночной цене
        {
            string endpoint = "https://testnet.binancefuture.com/fapi/v1/order";
            long timestamp = (long)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds;
            var parameters = new Dictionary<string, string>
        {
            { "symbol", symbol },  // Symbol of the futures contract
            { "side", side },        // SIDE: BUY or SELL
            { "type", type },     // TYPE: MARKET, LIMIT, STOP_MARKET etc.
            {"timeInForce", BaseInfo.time },
            { "quantity", quantity.ToString().Replace(",",".") },  // Quantity to buy or sell\
            {"stopPrice",stopprice.ToString().Replace(",",".") },
            { "timestamp", timestamp.ToString() },  // Current timestamp
            };
            var payload = BaseInfo.CreateQueryString(parameters);
            var signature = BaseInfo.CalculateSignature(BaseInfo.TESTsecretKey, payload);
            var requestUri = $"{endpoint}?{payload}&signature={signature}";
            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("X-MBX-APIKEY", BaseInfo.TESTapiKey);

            var response = await client.PostAsync(requestUri, null);
            var content = await response.Content.ReadAsStringAsync();
            var orderResponse = JsonConvert.DeserializeObject<Models.FuturesAssetBalance>(content);
            var order = orderResponse.orderId;
            Console.WriteLine(content);
            Console.WriteLine(order + " Open SL");
            return order;
        }
        public static async Task<double> PlaceBuyOrderStopMarket2(string symbol, string side, string type, double quantity, double stopprice) //При достижении цены в 7100 активируется заявка на покупку по рыночной цене
        {
            string endpoint = "https://testnet.binancefuture.com/fapi/v1/order";
            long timestamp = (long)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds;
            var parameters = new Dictionary<string, string>
        {
            { "symbol", symbol },  // Symbol of the futures contract
            { "side", side },        // SIDE: BUY or SELL
            { "type", type },     // TYPE: MARKET, LIMIT, STOP_MARKET etc.
            {"timeInForce", BaseInfo.time },
            { "quantity", quantity.ToString().Replace(",",".") },  // Quantity to buy or sell\
            {"stopPrice",stopprice.ToString().Replace(",",".") },
            { "timestamp", timestamp.ToString() },  // Current timestamp
            };
            var payload = BaseInfo.CreateQueryString(parameters);
            var signature = BaseInfo.CalculateSignature(BaseInfo.TESTsecretKey, payload);
            var requestUri = $"{endpoint}?{payload}&signature={signature}";
            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("X-MBX-APIKEY", BaseInfo.TESTapiKey);
            var response = await client.PostAsync(requestUri, null);
            var content = await response.Content.ReadAsStringAsync();
            var orderResponse = JsonConvert.DeserializeObject<Models.FuturesAssetBalance>(content);
            var order = orderResponse.orderId;
            Console.WriteLine(content);
            Console.WriteLine(order + " Open TP");
            return order;
        }
        public static async Task CancelOrder(string symbol, double orderId)
        {
            long timestamp = (long)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds;
            string endpointPath = "/fapi/v1/order";
            var parameters = new Dictionary<string, string>
        {
            { "symbol", symbol },  // Symbol of the futures contract
            { "timestamp", timestamp.ToString() },  // Current timestamp
                {"orderId",orderId.ToString() },
            };
            var payload = BaseInfo.CreateQueryString(parameters);
            var signature = BaseInfo.CalculateSignature(BaseInfo.TESTsecretKey, payload);
            var requestUri = $"{BaseInfo.TESTBASEURL}{endpointPath}?{payload}&signature={signature}";
            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("X-MBX-APIKEY", BaseInfo.TESTapiKey);

            var response = await client.DeleteAsync(requestUri);

            var content = await response.Content.ReadAsStringAsync();
            Console.WriteLine("Order Cancel OK : " + symbol);
        }
        public static async Task<List<double>> GetOpenOrders(string symbol)
        {
            string endpoint = "/fapi/v1/openOrders";
            long timestamp = (long)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds;
            var parameters = new Dictionary<string, string>
            {
                {"symbol", symbol },
                { "timestamp", timestamp.ToString() },  // Current timestamp
            };
            var payload = BaseInfo.CreateQueryString(parameters);
            var signature = BaseInfo.CalculateSignature(BaseInfo.TESTsecretKey, payload);

            var requestUri = $"{BaseInfo.TESTBASEURL}{endpoint}?{payload}&signature={signature}";
            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("X-MBX-APIKEY", BaseInfo.TESTapiKey);

            HttpResponseMessage response = await client.GetAsync(requestUri);
            string jsonResponse = await response.Content.ReadAsStringAsync();

            List<Models.FuturesAssetBalance> orderResponse = JsonConvert.DeserializeObject<List<Models.FuturesAssetBalance>>(jsonResponse);
            List<double> opens = new List<double>();
            foreach (var test in orderResponse)
            {
                opens.Add(test.orderId);
            }

            return opens;


        }
    }
}
