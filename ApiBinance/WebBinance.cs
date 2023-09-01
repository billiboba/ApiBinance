﻿using Nancy;
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
        public class FuturesAccountInfo
        {
            [JsonProperty("profit")]
            public List<FuturesAssetBalance> Profit { get; init; }

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
        public class Profit
        {
            [JsonProperty("unRealizedProfit")]
            public double unRealizedProfit { get; init; }
        }

        //public static async Task GetOpenPositionFutures() //Информация о сделке(Профит по сделке)
        //{
        //    long timestamp = (long)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds;
        //    string queryString = $"timestamp={timestamp}";
        //    string signature = BaseInfo.CalculateSignature(BaseInfo.secretKey, queryString);
        //    string endpoint = "/fapi/v2/positionRisk";
        //    string url = $"{BaseInfo.baseUrl}{endpoint}?{queryString}&signature={signature}";
        //    HttpClient client = new HttpClient();
        //    client.DefaultRequestHeaders.Add("X-MBX-APIKEY", BaseInfo.apiKey);
        //    HttpResponseMessage response = await client.GetAsync(url);
        //    string responseBody = await response.Content.ReadAsStringAsync();
        //    List<FuturesAssetBalance> positions = JsonConvert.DeserializeObject<List<FuturesAssetBalance>>(responseBody);
        //    foreach (FuturesAssetBalance position in positions)
        //    {
        //        if (position.EntryPrice != 0)
        //        {
        //            Console.WriteLine($"Символ: {position.symbol}, Profit: {position.unRealizedProfit}");
        //        }
        //    }
        //}
        public static async Task<Dictionary<string,double>> GetOpenPositionFutures() //Информация о сделке(Профит по сделке)//Переделать, скорее всего нужно, чтобы данные выводились в массив.
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
            var test = new Dictionary<string, double>();
            foreach (FuturesAssetBalance position in positions)
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
            return test;
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