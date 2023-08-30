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
                await WebBinance.GetOpenPositionFutures();
                Thread.Sleep(10000);
            }

        }
    }
}