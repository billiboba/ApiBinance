using Nancy;
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
        public class FuturesAccountInfo
        {
            [JsonProperty("assets")]
            public List<FuturesAssetBalance> Balances { get; init; }

        }

        public class FuturesAssetBalance
        {
            [JsonProperty("asset")]
            public string Asset { get; init; }

            [JsonProperty("symbol")]
            public string symbol { get; init; }

            [JsonProperty("positionAmt")]
            public decimal PositionAmt { get; init; }

            [JsonProperty("entryPrice")]
            public decimal EntryPrice { get; init; }

            [JsonProperty("unRealizedProfit")]
            public double unRealizedProfit { get; init; }

            [JsonProperty("totalMarginBalance")]
            public double totalMarginBalance { get; init; }
        } 
        public static async Task GetOpenPositionFutures() //Информация о сделке(Профит по сделке)
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
            List<FuturesAssetBalance> positions = JsonConvert.DeserializeObject<List<FuturesAssetBalance>>(responseBody);
            //Console.WriteLine(responseBody);
            foreach (FuturesAssetBalance position in positions)
            {
                if (position.EntryPrice != 0)
                {
                    Console.WriteLine($"Символ: {position.symbol}, Profit: {position.unRealizedProfit}, Средняя цена: {position.EntryPrice}");
                }
            }
        }

        //public static async Task<double> GetAccountInfo() //Get info account Binance
        //{
        //    long timestamp = (long)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds;
        //    string queryString = $"timestamp={timestamp}";
        //    string signature = BaseInfo.CalculateSignature(BaseInfo.secretKey, queryString);
        //    string endpoint = "/fapi/v2/account";
        //    string url = $"{BaseInfo.baseUrl}{endpoint}?{queryString}&signature={signature}";
        //    string asset = "USDT";
        //    HttpClient client = new HttpClient();
        //    client.DefaultRequestHeaders.Add("X-MBX-APIKEY", BaseInfo.apiKey);

        //    HttpResponseMessage response = await client.GetAsync(url);
        //    string responseBody = await response.Content.ReadAsStringAsync();
        //    if (response.IsSuccessStatusCode)
        //    {
        //        var jsonResponse = await response.Content.ReadAsStringAsync();
        //        var accountInfo = JsonConvert.DeserializeObject<FuturesAccountInfo>(jsonResponse);
        //        var balance = accountInfo.Balances.FirstOrDefault(b => b.Asset == asset);
        //        Console.WriteLine(response.StatusCode);
        //        Console.WriteLine(responseBody);
        //        //return balance.TotalUnrealizedProfit;
        //        return balance.TotalWalletBalance; //Вывод баланса
        //    }
        //    else
        //    {
        //        throw new Exception("Ошибка запроса");
        //    }
        //}
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
                var accountInfo = JsonConvert.DeserializeObject<FuturesAssetBalance>(jsonResponse);
                return accountInfo.totalMarginBalance;
            }
            else
            {
                throw new Exception("Ошибка запроса");
            }
        }
    }
}
