using ApiBinance.ApiBinance;
using CryptoExchange.Net.CommonObjects;
using Newtonsoft.Json;
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
        public static void GetFuturesSimbolsBybit() //Получение всех символов
        {
            using var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Get, $"{BaseInfoBybit.BaseUrl}/v2/public/tickers");
            var response = client.SendAsync(request).Result;
            var jsonString = response.Content.ReadAsStringAsync().Result;
            dynamic responseObject = JsonConvert.DeserializeObject(jsonString);
            List<FuturesModels> futuresModels = responseObject.result.ToObject<List<FuturesModels>>();
            foreach (var futuresModel in futuresModels)
            {
                Console.WriteLine($"{futuresModel.symbol} : {futuresModel.last_Price}");
            }
            Console.WriteLine(futuresModels.Count);
        }
        public static void GetHistoryCurrency() //Получение истории по каждому символу
        {
            var parameters = new Dictionary<string, object>
            {
                {"symbol", "BTCUSDT"},//меняется 
                {"limit", "10"},//меняется максимум 1000
                {"interval", "D"},//меняется [1, 3, 5, 15, 30, 60, 120, 240, 360, 720, D, M, W]
                {"category","linear" }
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
        public static void MarketPlaceOrder()//Создание ордера по рыночной цене
        {
            var parameters = new Dictionary<string, object>
            {
            {"category", "linear"},
            {"symbol", "OPUSDT"},
            {"side", "Buy"},
            {"positionIdx", 0},
            {"orderType", "Market"},
            {"qty", "0.3"},
            {"tpslMode","Partial" },
            {"tpOrderType","limit" },
            {"slOrderType" , "Limit"},
            {"tpLimitPrice","3.76" },
            {"slLimitPrice", "3.7039" },
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

        public static void TakeProfitOrder()//Создание тейк профита и стоп лосс по открытой позиции
        {
            var parameters = new Dictionary<string, object>
            {
            {"category", "linear"},
            {"symbol", "OPUSDT"},
            {"takeProfit","3.71" },
            { "stopLoss", "3.66"},
            {"tpTriggerBy","MarkPrice" },
            {"slTriggerBy" ,"IndexPrice"},
            {"tpslMode","Partial" },
            {"tpOrderType","limit" },
            {"slOrderType" , "Limit"},
            {"tpSize","0.3" },
            {"slSize","0.3" },
            {"tpLimitPrice","3.71" },
            {"slLimitPrice", "3.66" },
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

    }
}
