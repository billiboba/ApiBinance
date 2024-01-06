using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ApiBinance.ApiByBit
{
    public class BybitAccount
    {
        private const string ApiKey = "***";
        private const string ApiSecret = "***";
        private static readonly string Timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds().ToString();
        private const string RecvWindow = "5000";

        public void PlaceOrder()
        {
            var parameters = new Dictionary<string, object>
            {
                {"category", "linear"},
                {"symbol", "BTCUSDT"},
                {"side", "Buy"},
                {"positionIdx", 0},
                {"orderType", "Market"},
                {"qty", "0.001"},
                {"timeInForce", "GTC"}
            };

            string signature = GeneratePostSignature(parameters);
            string jsonPayload = JsonConvert.SerializeObject(parameters);

            using var client = new HttpClient();
            HttpRequestMessage request = new(HttpMethod.Post, "https://api-testnet.bybit.com/v5/order/create")
            {
                Content = new StringContent(jsonPayload, Encoding.UTF8, "application/json")
            };

            request.Headers.Add("X-BAPI-API-KEY", ApiKey);
            request.Headers.Add("X-BAPI-SIGN", signature);
            request.Headers.Add("X-BAPI-SIGN-TYPE", "2");
            request.Headers.Add("X-BAPI-TIMESTAMP", Timestamp);
            request.Headers.Add("X-BAPI-RECV-WINDOW", RecvWindow);

            var response = client.SendAsync(request).Result;
            Console.WriteLine(response.Content.ReadAsStringAsync().Result);
        }

        public void GetWalletBalance()
        {
            var parameters = new Dictionary<string, object>
    {
        //{"coin", "USDT"},
                {"accountType", "UNIFIED"}
    };

            string signature = GenerateGetSignature(parameters);
            string queryString = GenerateQueryString(parameters);

            using var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Get, $"https://api-testnet.bybit.com/v5/account/wallet-balance?{queryString}");
            //var request = new HttpRequestMessage(HttpMethod.Get, $"https://api.bybit.com/v5/asset/transfer/query-account-coins-balance?{queryString}");
            request.Headers.Add("X-BAPI-API-KEY", ApiKey);
            request.Headers.Add("X-BAPI-SIGN", signature);
            //request.Headers.Add("X-BAPI-SIGN-TYPE", "2");
            request.Headers.Add("X-BAPI-TIMESTAMP", Timestamp);
            request.Headers.Add("X-BAPI-RECV-WINDOW", RecvWindow);

            var response = client.SendAsync(request).Result;
            Console.WriteLine(response.Content.ReadAsStringAsync().Result);
        }

        private static string GeneratePostSignature(IDictionary<string, object> parameters)
        {
            string paramJson = JsonConvert.SerializeObject(parameters);
            string rawData = Timestamp + ApiKey + RecvWindow + paramJson;

            using var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(ApiSecret));
            var signature = hmac.ComputeHash(Encoding.UTF8.GetBytes(rawData));
            return BitConverter.ToString(signature).Replace("-", "").ToLower();
        }

        private static string GenerateGetSignature(Dictionary<string, object> parameters)
        {
            string queryString = GenerateQueryString(parameters);
            Console.WriteLine(queryString);
            string rawData = Timestamp + ApiKey + RecvWindow + queryString;
            Console.WriteLine(Timestamp);
            return ComputeSignature(rawData);
        }

        private static string ComputeSignature(string data)
        {
            using var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(ApiSecret));
            byte[] signature = hmac.ComputeHash(Encoding.UTF8.GetBytes(data));
            return BitConverter.ToString(signature).Replace("-", "").ToLower();
        }

        private static string GenerateQueryString(Dictionary<string, object> parameters)
        {
            return string.Join("&", parameters.Select(p => $"{p.Key}={p.Value}"));
        }
    }
}
