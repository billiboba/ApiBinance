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
                List<string> boba = await Simulate_Trading.TESTGetOpenPositionFutures();
                for(int i = 0; i < boba.Count; i++)
                {
                    Console.WriteLine(boba[i]);
                }
                if (boba.Count != 0)
                {
                    for (int i = 0; i < boba.Count; i++)
                    {
                        if (boba[i] != null)
                        {
                            continue;
                        }
                        else
                        {
                            await OpenPositionOrders.OpenPos();
                        }
                    }
                }
                else
                {
                    await OpenPositionOrders.OpenPos();
                }
                Thread.Sleep(5000);
            };
        }
    }
}//Open positions! 