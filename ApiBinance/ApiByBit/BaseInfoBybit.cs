using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Cryptocurrency.ApiByBit
{
    public class BaseInfoBybit
    {
        public const string BaseUrl = "https://api.bybit.com/";
        public const string ApiKey = "LSMPr0sfJqRJuMBHy9";
        public const string ApiSecret = "5Ut2XJTXdxFY51LpGNeCbVaDWUl0AOZRGbrq";
        public static readonly string Timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds().ToString();
        public const string RecvWindow = "5000";

        public static string GeneratePostSignature(IDictionary<string, object> parameters)
        {
            string paramJson = JsonConvert.SerializeObject(parameters);
            string rawData = Timestamp + ApiKey + RecvWindow + paramJson;

            using var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(ApiSecret));
            var signature = hmac.ComputeHash(Encoding.UTF8.GetBytes(rawData));
            return BitConverter.ToString(signature).Replace("-", "").ToLower();
        }

        public static string GenerateGetSignature(Dictionary<string, object> parameters)
        {
            string queryString = GenerateQueryString(parameters);
            Console.WriteLine(queryString);
            string rawData = Timestamp + ApiKey + RecvWindow + queryString;
            Console.WriteLine(Timestamp);
            return ComputeSignature(rawData);
        }

        public static string ComputeSignature(string data)
        {
            using var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(ApiSecret));
            byte[] signature = hmac.ComputeHash(Encoding.UTF8.GetBytes(data));
            return BitConverter.ToString(signature).Replace("-", "").ToLower();
        }

        public static string GenerateQueryString(Dictionary<string, object> parameters)
        {
            return string.Join("&", parameters.Select(p => $"{p.Key}={p.Value}"));
        }
    }
}
