using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ApiBinance
{
    public class BaseInfo
    {
        //For Main Account
        public const string apiKey = "eAS8stCpELklCHEyukyeWQU5BvRERozoRNQ793sUN8mf7roQYUGWIATw7sh9JYoE";
        public const string secretKey = "NgpNCSmYTkiwlJ0BJJaP3YWo9bPzzpvhCxbgIMwHaihCz8RSCtN1LIWyDLwhFG32";
        public const string baseUrl = "https://fapi.binance.com";
        public const string urlPosition = "https://api.binance.com/api/v3/account";
        public const string time = "GTC";
        //For Test Account
        public const string TESTapiKey = "32029817002256cb96f68ac1444e27583c36cf83277fa21a70ea43c5155cf6f1";
        public const string TESTsecretKey = "6e35f4e31c8266eb4820906fc3c1a82432db0a42a4ed7c03d662b167e23c5a05";
        public const string TESTBASEURL = "https://testnet.binancefuture.com";
        public static string CalculateSignature(string secretKey, string payload)
        {
            var encoding = new UTF8Encoding();
            byte[] keyBytes = encoding.GetBytes(secretKey);
            byte[] payloadBytes = encoding.GetBytes(payload);

            using (var hmacSha256 = new HMACSHA256(keyBytes))
            {
                byte[] signatureBytes = hmacSha256.ComputeHash(payloadBytes);
                return BitConverter.ToString(signatureBytes).Replace("-", "").ToLower();
            }
        }
        public static string CreateQueryString(Dictionary<string, string> parameters)
        {
            var stringBuilder = new StringBuilder();
            foreach (var parameter in parameters)
            {
                stringBuilder.Append($"{parameter.Key}={parameter.Value}&");
            }
            var queryString = stringBuilder.ToString().TrimEnd('&');
            return queryString;
        }
    }
}
