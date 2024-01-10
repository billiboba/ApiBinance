using Bybit.Api.Helpers;
using Cryptocurrency.ApiByBit;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using static Cryptocurrency.ApiByBit.ModelsBybit;

namespace ApiBinance.ApiByBit
{
    public class BybitAccount
    {
        public void GetWalletBalance()
        {
            var parameters = new Dictionary<string, object>
        {
           {"coin", "USDT"},
            {"accountType", "UNIFIED"}
        };
            string signature = BaseInfoBybit.GenerateGetSignature(parameters);
            string queryString = BaseInfoBybit.GenerateQueryString(parameters);

            using var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Get, $"{BaseInfoBybit.BaseUrl}v5/account/wallet-balance?{queryString}");
            request.Headers.Add("X-BAPI-API-KEY", BaseInfoBybit.ApiKey);
            request.Headers.Add("X-BAPI-SIGN", signature);
            request.Headers.Add("X-BAPI-TIMESTAMP", BaseInfoBybit.Timestamp);
            request.Headers.Add("X-BAPI-RECV-WINDOW", BaseInfoBybit.RecvWindow);

            var response = client.SendAsync(request).Result;
            var jsonString = response.Content.ReadAsStringAsync().Result;
            Console.WriteLine(jsonString);
            dynamic responseObject = JsonConvert.DeserializeObject(jsonString);
            dynamic futuresModel = responseObject.result.list[0];
            string totalEquity = futuresModel.totalEquity;//Получаем общий баланс
            string totalAvailableBalance = futuresModel.totalAvailableBalance;//Получаем доступный баланс
            Console.WriteLine(totalEquity);
            Console.WriteLine(totalAvailableBalance);
        }
    }
}
