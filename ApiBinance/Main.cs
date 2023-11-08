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
                List<string> symbols = new List<string>() { "BTCUSDT", "ETHUSDT", "MKRUSDT" };
                for(int i = 0; i < symbols.Count; i++)
                {
                    Console.WriteLine(symbols[i]);
                    await OpenPositionOrders.CheckOpenPos(symbols[i]);
                }
                
            }
        }
    }
}