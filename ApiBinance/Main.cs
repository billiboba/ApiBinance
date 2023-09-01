using ApiBinance;
using Newtonsoft.Json;
using static ApiBinance.WebBinance;

namespace BinanceFuturesAccount
{
    public class Program 
    {
        public static async Task Main()
        {
            
            while (true)
            {
                Dictionary<string, double> open = await WebBinance.GetOpenPositionFutures();
                foreach(var pos in open)
                {
                    Console.WriteLine(pos.Key + pos.Value);
                }
                Thread.Sleep(1000);
            }

        }
    }
}