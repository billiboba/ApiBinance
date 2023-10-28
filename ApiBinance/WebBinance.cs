using Nancy;
using Nancy.Json;
using Nancy.Responses;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ApiBinance
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
                if(position.EntryPrice != 0)
                {
                    test.Add(position.symbol, position.unRealizedProfit);
                }
            }
            foreach(var open in test)
            {
                Console.WriteLine(open.Key + ": " + open.Value);
            }
        }

        public static async Task<double> GetAccountBalance() //Get info account Binance
        {
            long timestamp = (long)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds;
            string queryString = $"timestamp={timestamp}";
            string signature = BaseInfo.CalculateSignature(BaseInfo.secretKey, queryString);
            string endpoint = "/fapi/v2/account";
            string url = $"{BaseInfo.baseUrl}{endpoint}?{queryString}&signature={signature}";
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("X-MBX-APIKEY", BaseInfo.apiKey);
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
    }
}
