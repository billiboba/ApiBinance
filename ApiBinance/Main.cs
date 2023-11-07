using ApiBinance;
using Newtonsoft.Json;
using static ApiBinance.WebBinance;

namespace BinanceFuturesAccount
{
    public class Program 
    {
        public static async Task Main()
        {
            double[] OrderId = await OpenPositionOrders.OpenPositionWithOrders();

            int x;
            while (true)
            {
                List<double> orderId = await Simulate_Trading.GetOpenOrders("BTCUSDT");
                x = orderId.Count;
                if(x < 2) {
                
                    if (orderId[0] == null)
                    {
                        await Simulate_Trading.CancelOrder("BTCUSDT", orderId[1]);
                        orderId.Remove(1);
                        x--;
                    }
                    else
                    {
                        await Simulate_Trading.CancelOrder("BTCUSDT", orderId[0]);
                        orderId.Remove(0);
                        x--;
                    }
                    break;
                }
                    Thread.Sleep(5000);
            };
        }
    }
}