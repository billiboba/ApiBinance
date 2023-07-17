﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ApiBinance
{
    public class BaseInfo
    {
        public const string apiKey = "api_key";
        public const string secretKey = "secret_key";
        public const string baseUrl = "https://fapi.binance.com";
        public const string urlPosition = "https://api.binance.com/api/v3/account";

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

    }
}