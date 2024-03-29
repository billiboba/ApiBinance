﻿using ApiBinance.ApiBinance;
using ApiSharp.Throttling.Abstractions;
using Bybit.Api.Helpers;
using CryptoExchange.Net.CommonObjects;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Cryptocurrency.ApiByBit.ModelsBybit;

namespace Cryptocurrency.ApiByBit
{
    public class GetOrdersBybit
    {
        public static void GetDataCurrency(string symbol, string limit, string timeframe) //Получение данных по каждому символу
        {
            var parameters = new Dictionary<string, object>
            {
                {"symbol", symbol},//меняется 
                {"limit", limit},//меняется максимум 1000
                {"interval", timeframe},//меняется [1, 3, 5, 15, 30, 60, 120, 240, 360, 720, D, M, W]
                {"category","inverse" }
            };
            string queryString = BaseInfoBybit.GenerateQueryString(parameters);

            using var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Get, $"{BaseInfoBybit.BaseUrl}v5/market/kline?{queryString}");
            request.Headers.Add("X-BAPI-API-KEY", BaseInfoBybit.ApiKey);
            request.Headers.Add("X-BAPI-TIMESTAMP", BaseInfoBybit.Timestamp);
            request.Headers.Add("X-BAPI-RECV-WINDOW", BaseInfoBybit.RecvWindow);

            var response = client.SendAsync(request).Result;
            var jsonString = response.Content.ReadAsStringAsync().Result;
            dynamic responseObject = JsonConvert.DeserializeObject(jsonString);
            Console.WriteLine(jsonString.ToString());
        }
        public static void GetOpenOrdersBybit(string symbol) //Получение открытых позиций по символу
        {
            var parameters = new Dictionary<string, object>
            {
                {"category", "linear"},
                {"symbol",symbol }
            };
            string signature = BaseInfoBybit.GenerateGetSignature(parameters);
            string queryString = BaseInfoBybit.GenerateQueryString(parameters);

            using var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Get, $"{BaseInfoBybit.BaseUrl}v5/position/list?{queryString}");
            request.Headers.Add("X-BAPI-API-KEY", BaseInfoBybit.ApiKey);
            request.Headers.Add("X-BAPI-SIGN", signature);
            request.Headers.Add("X-BAPI-TIMESTAMP", BaseInfoBybit.Timestamp);
            request.Headers.Add("X-BAPI-RECV-WINDOW", BaseInfoBybit.RecvWindow);
            var response = client.SendAsync(request).Result;
            var jsonString = response.Content.ReadAsStringAsync().Result;
            Console.WriteLine(jsonString);
        }
        public static void MarketPlaceOrder(string symbol, string side,string quantity)//Создание ордера по рыночной цене
        {
            var parameters = new Dictionary<string, object>
            {
            {"category", "linear"},
            {"symbol", symbol},
            {"side", side},
            {"positionIdx", 0},
            {"orderType", "Market"},
            {"qty", quantity},
            };

            string signature = BaseInfoBybit.GeneratePostSignature(parameters);
            string jsonPayload = JsonConvert.SerializeObject(parameters);

            using var client = new HttpClient();
            HttpRequestMessage request = new(HttpMethod.Post, "https://api.bybit.com/v5/order/create")
            {
                Content = new StringContent(jsonPayload, Encoding.UTF8, "application/json")
            };
            request.Headers.Add("X-BAPI-API-KEY", BaseInfoBybit.ApiKey);
            request.Headers.Add("X-BAPI-SIGN", signature);
            request.Headers.Add("X-BAPI-SIGN-TYPE", "2");
            request.Headers.Add("X-BAPI-TIMESTAMP", BaseInfoBybit.Timestamp);
            request.Headers.Add("X-BAPI-RECV-WINDOW", BaseInfoBybit.RecvWindow);

            var response = client.SendAsync(request).Result;
            Console.WriteLine(response.Content.ReadAsStringAsync().Result);
        }
        public static void LimitPlaceOrder()//Создание ордера по лимитной цене
        {
            var parameters = new Dictionary<string, object>
            {
                {"category", "linear"},
                {"symbol", "OPUSDT"},
                {"side", "Buy"},
                {"positionIdx", 0},
                {"orderType", "Limit"},
                {"qty", "0.3"},
                {"price", "3.64"},
                {"timeInForce", "GTC"}
            };

            string signature = BaseInfoBybit.GeneratePostSignature(parameters);
            string jsonPayload = JsonConvert.SerializeObject(parameters);

            using var client = new HttpClient();
            HttpRequestMessage request = new(HttpMethod.Post, "https://api.bybit.com/v5/order/create")
            {
                Content = new StringContent(jsonPayload, Encoding.UTF8, "application/json")
            };
            request.Headers.Add("X-BAPI-API-KEY", BaseInfoBybit.ApiKey);
            request.Headers.Add("X-BAPI-SIGN", signature);
            request.Headers.Add("X-BAPI-SIGN-TYPE", "2");
            request.Headers.Add("X-BAPI-TIMESTAMP", BaseInfoBybit.Timestamp);
            request.Headers.Add("X-BAPI-RECV-WINDOW", BaseInfoBybit.RecvWindow);

            var response = client.SendAsync(request).Result;
            Console.WriteLine(response.Content.ReadAsStringAsync().Result);
        }

        public static void TakeProfitOrder(string symbol, string quantity,string takeProfit,string stopLoss)//Создание тейк профита и стоп лосс по открытой позиции
        {
            var parameters = new Dictionary<string, object>
            {
            {"category", "linear"},
            {"symbol", symbol},
            {"takeProfit",takeProfit },//OPENPRICE + 0.4*ATR(LONG) или OPENPRICE - 0.4*ATR(SHORT)
            { "stopLoss",  stopLoss},//OPENPRICE - 0.1 * ATR(LONG)  OPENPRICE + 0.1 * ATR(SHORT)
            {"tpTriggerBy","MarkPrice" },
            {"slTriggerBy" ,"IndexPrice"},
            {"tpslMode","Partial" },
            {"tpOrderType","limit" },
            {"slOrderType" , "Limit"},
            {"tpSize",quantity },
            {"slSize",quantity },
            {"tpLimitPrice",takeProfit },
            {"slLimitPrice", stopLoss },
            {"timeInForce", "GTC"}
            };

            string signature = BaseInfoBybit.GeneratePostSignature(parameters);
            string jsonPayload = JsonConvert.SerializeObject(parameters);

            using var client = new HttpClient();
            HttpRequestMessage request = new(HttpMethod.Post, "https://api.bybit.com/v5/position/trading-stop")
            {
                Content = new StringContent(jsonPayload, Encoding.UTF8, "application/json")
            };
            request.Headers.Add("X-BAPI-API-KEY", BaseInfoBybit.ApiKey);
            request.Headers.Add("X-BAPI-SIGN", signature);
            request.Headers.Add("X-BAPI-SIGN-TYPE", "2");
            request.Headers.Add("X-BAPI-TIMESTAMP", BaseInfoBybit.Timestamp);
            request.Headers.Add("X-BAPI-RECV-WINDOW", BaseInfoBybit.RecvWindow);

            var response = client.SendAsync(request).Result;
            Console.WriteLine(response.Content.ReadAsStringAsync().Result);
        }
        public static void CancelOrder() //Закрытие ордера обязательно передавать в параметры orderId
        {
            var parameters = new Dictionary<string, object>
            {
                {"category", "linear"},
                {"symbol", "OPUSDT"},
                {"orderId","ef97610a-ec66-4b21-b833-5d095fdec4ca" }
            };

            string signature = BaseInfoBybit.GeneratePostSignature(parameters);
            string jsonPayload = JsonConvert.SerializeObject(parameters);

            using var client = new HttpClient();
            HttpRequestMessage request = new(HttpMethod.Post, "https://api.bybit.com/v5/order/cancel")
            {
                Content = new StringContent(jsonPayload, Encoding.UTF8, "application/json")
            };
            request.Headers.Add("X-BAPI-API-KEY", BaseInfoBybit.ApiKey);
            request.Headers.Add("X-BAPI-SIGN", signature);
            request.Headers.Add("X-BAPI-SIGN-TYPE", "2");
            request.Headers.Add("X-BAPI-TIMESTAMP", BaseInfoBybit.Timestamp);
            request.Headers.Add("X-BAPI-RECV-WINDOW", BaseInfoBybit.RecvWindow);

            var response = client.SendAsync(request).Result;
            Console.WriteLine(response.Content.ReadAsStringAsync().Result);
        }
        public static void GetOpenPositions() //Получение открытых позиций в реальном времени
        {
            var parameters = new Dictionary<string, object>
            {
                {"category", "linear"},
                {"settleCoin","USDT" }
            };
            string signature = BaseInfoBybit.GenerateGetSignature(parameters);
            string queryString = BaseInfoBybit.GenerateQueryString(parameters);

            using var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Get, $"{BaseInfoBybit.BaseUrl}v5/position/list?{queryString}");
            request.Headers.Add("X-BAPI-API-KEY", BaseInfoBybit.ApiKey);
            request.Headers.Add("X-BAPI-SIGN", signature);
            request.Headers.Add("X-BAPI-TIMESTAMP", BaseInfoBybit.Timestamp);
            request.Headers.Add("X-BAPI-RECV-WINDOW", BaseInfoBybit.RecvWindow);
            var response = client.SendAsync(request).Result;
            var jsonString = response.Content.ReadAsStringAsync().Result;
            Console.WriteLine(jsonString);
        }
        public static double GetVolatily(string symbol) //Получение данных по каждому символу
        {
            var parameters = new Dictionary<string, object>
            {
                {"symbol", symbol},
                {"category","linear" }
            };
            string queryString = BaseInfoBybit.GenerateQueryString(parameters);

            using var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Get, $"{BaseInfoBybit.BaseUrl}v5/market/tickers?{queryString}");
            var response = client.SendAsync(request).Result;
            string jsonString = response.Content.ReadAsStringAsync().Result;
            var responseObject = JObject.Parse(jsonString);
            var turnover24h = (string)responseObject["result"]["list"][0]["turnover24h"];
            return turnover24h.ToDouble();
        }
        public static List<string> GetFuturesSymbolsBybit() // Получение всех символов
        {
            using var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Get, $"{BaseInfoBybit.BaseUrl}/v2/public/tickers");
            var response = client.SendAsync(request).Result;
            var jsonString = response.Content.ReadAsStringAsync().Result;
            dynamic responseObject = JsonConvert.DeserializeObject(jsonString);
            List<FuturesModels> futuresModels = responseObject.result.ToObject<List<FuturesModels>>();
            List<string> symbols = new List<string>();
            foreach (var futuresModel in futuresModels)
            {
                symbols.Add(futuresModel.symbol);
            }
            return symbols;
        }
        public static void GetClosedPositions() //Получение закрытых позиций.
        {
            var parameters = new Dictionary<string, object>
            {
                {"category", "linear"},
            };
            string signature = BaseInfoBybit.GenerateGetSignature(parameters);
            string queryString = BaseInfoBybit.GenerateQueryString(parameters);

            using var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Get, $"{BaseInfoBybit.BaseUrl}v5/execution/list?{queryString}");
            request.Headers.Add("X-BAPI-API-KEY", BaseInfoBybit.ApiKey);
            request.Headers.Add("X-BAPI-SIGN", signature);
            request.Headers.Add("X-BAPI-TIMESTAMP", BaseInfoBybit.Timestamp);
            request.Headers.Add("X-BAPI-RECV-WINDOW", BaseInfoBybit.RecvWindow);
            var response = client.SendAsync(request).Result;
            var jsonString = response.Content.ReadAsStringAsync().Result;
            Console.WriteLine(jsonString);
        }
    }
}
