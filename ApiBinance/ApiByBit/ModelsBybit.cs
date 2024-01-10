using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cryptocurrency.ApiByBit
{
    class ModelsBybit
    {
        public class FuturesModels
        {
            [JsonProperty("totalWalletBalance")]
            public string totalWalletBalance { get; init; }
            
            [JsonProperty("totalMarginBalance")]
            public string totalMarginBalance { get; init; }
            
            [JsonProperty("symbol")]
            public string symbol { get; init; }
            
            [JsonProperty("last_price")]
            public string last_Price { get; init; }
            [JsonProperty("w")]
            public string totalEquity { get; set; }
        }
    }
}
