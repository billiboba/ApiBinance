using ApiBinance;
using Newtonsoft.Json;
using static ApiBinance.WebBinance;

namespace BinanceFuturesAccount
{
    public class Program 
    {
        public static async Task Main()
        {
            //await Simulate_Trading.TESTGetAccountBalance();
            //Console.WriteLine(aps);
            //await WebBinance.GetClosedPosition();
            while (true)
            {
                Dictionary<string, double> open = await Simulate_Trading.TESTGetOpenPositionFutures();
                foreach(var pos in open)
                {
                   Console.WriteLine(pos.Key + pos.Value);
                }
                Thread.Sleep(1000);
                
            }

        }
    }
}