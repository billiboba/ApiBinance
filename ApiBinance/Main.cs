using ApiBinance;
using Newtonsoft.Json;
using static ApiBinance.WebBinance;

namespace BinanceFuturesAccount
{
    public class Program 
    {
        public static async Task Main()
        {
            //await Simulate_Trading.BuySell("BTCUSDT", "BUY", "MARKET", "0.01");
            await Simulate_Trading.EmergencyClosedPosition("3501090195");
            double test = await WebBinance.GetLiquid("BTCUSDT");
            Console.WriteLine(test);
            //await Simulate_Trading.PlaceBuyOrderStopMarket("BTCUSDT", "BUY", "MARKET", "0.01", "34600");
            //while (true)
            //{
            //    Dictionary<string, double> open = await Simulate_Trading.TESTGetOpenPositionFutures();
            //    foreach(var pos in open)
            //    {
            //       Console.WriteLine(pos.Key + pos.Value);
            //    }
            //    Thread.Sleep(1000);
                
            //}

        }
    }
}