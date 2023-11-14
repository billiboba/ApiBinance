using ApiBinance;
using Newtonsoft.Json;
using static ApiBinance.WebBinance;

namespace BinanceFuturesAccount
{
    public class Program 
    {
        public static async Task Main()
        {
            List<string> symbol = await WebBinance.GetLiquid();
            List<string> symbol2 = new List<string>();
            for (int i = 0; i < symbol.Count; i++)
            {
                if (symbol[i] == "BTCBUSD")
                {
                    symbol.RemoveAt(i);
                }
                symbol2.Add(symbol[i]);
            }
            while (true)
            {

                for (int i = 0; i < symbol2.Count; i++)
                {
                    if (symbol2[i] == "BTCBUSD")
                    {
                        symbol2.RemoveAt(i);
                    }
                    Console.WriteLine(symbol2[i] + " : " + i);
                    await OpenPositionOrders.CheckOpenPos(symbol2[i]);
                }
                Thread.Sleep(5000);
            }

            //double quan = await OpenPositionOrders.CheckQuantity("MKRUSDT");
            //double price = await WebBinance.GetFuturesPrice("MKRUSDT");

            //double sl = price - price * 0.01;
            //await Simulate_Trading.PlaceBuyOrderStopMarket("MKRUSDT", "SELL", "STOP_MARKET", quan, 1300);




        }
    }
}