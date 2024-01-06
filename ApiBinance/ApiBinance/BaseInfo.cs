using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

//LONG
//Level - 10%*ATR = SL
//Level + 40%*ATR = TP

//SHORT
//Level + 10%*ATR = SL
//Level - 40%*ATR = TP
namespace ApiBinance.ApiBinance
{
    public class BaseInfo
    {
        //For Main Account
        public const string apiKey = "***";
        public const string secretKey = "***";
        public const string baseUrl = "https://fapi.binance.com";
        public const string urlPosition = "https://api.binance.com/api/v3/account";
        public const string time = "GTC";
        //For Test Account
        public const string TESTapiKey = "***";
        public const string TESTsecretKey = "***";
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
