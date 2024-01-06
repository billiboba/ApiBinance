using Nancy;
using Nancy.Json;
using Nancy.Responses;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection.Metadata;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ApiBinance.ApiBinance
{
    public class WebBinance
    {
        public static async Task GetOpenPositionFutures() //Информация о сделке(Профит по сделке)//Переделать, скорее всего нужно, чтобы данные выводились в массив.
        {
            long timestamp = (long)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds;
            string queryString = $"timestamp={timestamp}";
            string signature = BaseInfo.CalculateSignature(BaseInfo.secretKey, queryString);
            string endpoint = "/fapi/v2/positionRisk";
            string url = $"{BaseInfo.baseUrl}{endpoint}?{queryString}&signature={signature}";

            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("X-MBX-APIKEY", BaseInfo.apiKey);

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
        }
        //https://fapi.binance.com/fapi/v1/ticker/24hr

        public static async Task<List<string>> GetFuturesSymbols()
        {
            long timestamp = (long)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds;
            string queryString = $"timestamp={timestamp}";
            string signature = BaseInfo.CalculateSignature(BaseInfo.secretKey, queryString);
            string endpoint = "/fapi/v1/ticker/24hr";
            string url = $"{BaseInfo.baseUrl}{endpoint}?{queryString}&signature={signature}";

            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("X-MBX-APIKEY", BaseInfo.TESTapiKey);

            HttpResponseMessage response = await client.GetAsync(url);
            string responseBody = await response.Content.ReadAsStringAsync();
            List<Models.FuturesAssetBalance> positions = JsonConvert.DeserializeObject<List<Models.FuturesAssetBalance>>(responseBody);
            var test = new List<string>();
            foreach (Models.FuturesAssetBalance position in positions)
            {

                test.Add(position.symbol);
            }
            return test;
        }
        public static async Task<double> GetFuturesPrice(string symbol)
        {
            string endpoint = "/fapi/v1/ticker/24hr";
            long timestamp = (long)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds;
            var parameters = new Dictionary<string, string>
            {
                {"symbol",symbol },
                { "timestamp", timestamp.ToString() },  // Current timestamp
            };
            var payload = BaseInfo.CreateQueryString(parameters);
            var signature = BaseInfo.CalculateSignature(BaseInfo.TESTsecretKey, payload);

            // Добавление подписи и ключа API в заголовок запроса
            var requestUri = $"{BaseInfo.TESTBASEURL}{endpoint}?{payload}&signature={signature}";
            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("X-MBX-APIKEY", BaseInfo.TESTapiKey);

            HttpResponseMessage response = await client.GetAsync(requestUri);
            string jsonResponse = await response.Content.ReadAsStringAsync();
            var orderResponse = JsonConvert.DeserializeObject<Models.FuturesAssetBalance>(jsonResponse);
            var price = orderResponse.lastPrice;
            return price;
        }
        public static async Task<double> GetAccountBalance() //Get info account Binance
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
                return accountInfo.totalMarginBalance;
            }
            else
            {
                throw new Exception("Ошибка запроса");
            }
        }

        public static async Task GetClosedPosition()
        {
            long timestamp = (long)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds;
            string baseUrl = "https://fapi.binance.com";
            //string endpoint = "/fapi/v1/trades";
            //string queryString = $"symbol=ZENUSDT&timestamp={GetTimestamp()}";
            string endpoint = "/fapi/v1/userTrades";
            string queryString = $"&timestamp={timestamp}";
            string signature = BaseInfo.CalculateSignature(BaseInfo.secretKey, queryString);
            string url = $"{baseUrl}{endpoint}?{queryString}&signature={signature}";
            Console.WriteLine(url);
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("X-MBX-APIKEY", BaseInfo.apiKey);

            HttpResponseMessage response = await client.GetAsync(url);
            string responseBody = await response.Content.ReadAsStringAsync();
            Console.WriteLine(responseBody);
            List<Models.FuturesAssetBalance> positions = JsonConvert.DeserializeObject<List<Models.FuturesAssetBalance>>(responseBody);
            var test = new Dictionary<string, double>();
            foreach (Models.FuturesAssetBalance position in positions)
            {
                Console.WriteLine(position.symbol + position.realizedPnl);
            }
        }

        public static async Task<List<string>> GetLiquid()
        {
            long timestamp = (long)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds;
            string queryString = $"timestamp={timestamp}";
            string signature = BaseInfo.CalculateSignature(BaseInfo.TESTsecretKey, queryString);
            string endpoint = "/fapi/v1/ticker/24hr";
            string url = $"{BaseInfo.TESTBASEURL}{endpoint}?{queryString}&signature={signature}";

            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("X-MBX-APIKEY", BaseInfo.TESTapiKey);

            HttpResponseMessage response = await client.GetAsync(url);
            string responseBody = await response.Content.ReadAsStringAsync();
            List<Models.FuturesAssetBalance> positions = JsonConvert.DeserializeObject<List<Models.FuturesAssetBalance>>(responseBody);
            var test = new List<string>();
            foreach (Models.FuturesAssetBalance position in positions)
            {
                if (position.quoteVolume > 10000000)
                {
                    test.Add(position.symbol);
                }

            }
            return test;
        }
    }
}
