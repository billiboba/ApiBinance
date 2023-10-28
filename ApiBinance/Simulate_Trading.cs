using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ApiBinance.WebBinance;

namespace ApiBinance
{
    class Simulate_Trading
    {
        public static async Task<double> TESTGetAccountBalance() //Get info account Binance
        {
            long timestamp = (long)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds;
            string queryString = $"timestamp={timestamp}";
            string signature = BaseInfo.CalculateSignature(BaseInfo.TESTsecretKey, queryString);
            string endpoint = "/fapi/v2/account";
            string url = $"{BaseInfo.TESTBASEURL}{endpoint}?{queryString}&signature={signature}";
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("X-MBX-APIKEY", BaseInfo.TESTapiKey);
            HttpResponseMessage response = await client.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                var jsonResponse = await response.Content.ReadAsStringAsync();
                var accountInfo = JsonConvert.DeserializeObject<Models.FuturesAssetBalance>(jsonResponse);
                Console.WriteLine(jsonResponse);
                return accountInfo.totalMarginBalance;
            }
            else
            {
                throw new Exception("Ошибка запроса");
            }
        }
        public static async Task<Dictionary<string, double>> TESTGetOpenPositionFutures() //Информация о сделке(Профит по сделке)//Переделать, скорее всего нужно, чтобы данные выводились в массив.
        {
            long timestamp = (long)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds;
            string queryString = $"timestamp={timestamp}";
            string signature = BaseInfo.CalculateSignature(BaseInfo.TESTsecretKey, queryString);
            string endpoint = "/fapi/v2/positionRisk";
            string url = $"{BaseInfo.TESTBASEURL}{endpoint}?{queryString}&signature={signature}";

            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("X-MBX-APIKEY", BaseInfo.TESTapiKey);

            HttpResponseMessage response = await client.GetAsync(url);
            string responseBody = await response.Content.ReadAsStringAsync();
            List<Models.FuturesAssetBalance> positions = JsonConvert.DeserializeObject<List<Models.FuturesAssetBalance>>(responseBody);
            var test = new Dictionary<string, double>();
            foreach (Models.FuturesAssetBalance position in positions)
            {
                if (position.EntryPrice != 0)
                {
                    test.Add(position.symbol, position.unRealizedProfit);
                }
            }
            foreach (var open in test)
            {
                Console.WriteLine(open.Key + ": " + open.Value);
            }
            return test;
        }
    

    }
}
