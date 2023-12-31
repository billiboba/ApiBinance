﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiBinance.ApiBinance
{
    class Models
    {
        public class FuturesAccountInfo
        {
            [JsonProperty("profit")]
            public List<FuturesAssetBalance> Profit { get; init; }

        }

        public class FuturesAssetBalance
        {
            [JsonProperty("asset")]
            public string Asset { get; init; }

            [JsonProperty("symbol")]
            public string symbol { get; init; }

            [JsonProperty("positionAmt")]
            public decimal PositionAmt { get; init; }

            [JsonProperty("entryPrice")]
            public double EntryPrice { get; init; }

            [JsonProperty("unRealizedProfit")]
            public double unRealizedProfit { get; init; }

            [JsonProperty("totalMarginBalance")]
            public double totalMarginBalance { get; init; }

            [JsonProperty("realizedPnl")]
            public double realizedPnl { get; init; }

            [JsonProperty("orderId")]
            public double orderId { get; init; }

            [JsonProperty("lastPrice")]
            public double lastPrice { get; init; }

            [JsonProperty("quoteVolume")]
            public double quoteVolume { get; init; }
        }
    }
}
