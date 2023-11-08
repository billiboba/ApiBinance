using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiBinance
{
    class OpenPositionOrders
    {
        //TAKE_PROFIT_MARKET если цена достигает указанной выше цены.

        public static async Task<double[]> OpenPositionWithOrders()
        {
            var order = await Simulate_Trading.BuySell("BTCUSDT", "BUY", "MARKET", "0.04");
            if (order == null)
            {
                Console.WriteLine("poshel nahui");
                return null;
            }

            var tasks = new Task<double>[]
            {
                    Simulate_Trading.PlaceBuyOrderStopMarket("BTCUSDT", "SELL", "STOP_MARKET", "0.04", "35000"),
                    Simulate_Trading.PlaceBuyOrderStopMarket2("BTCUSDT", "SELL", "TAKE_PROFIT_MARKET", "0.04", "36300")
            };
            var results = await Task.WhenAll(tasks);
            return results;
        }

        public static async Task OpenPos()
        {
            while (true)
            {
                await OpenPositionOrders.OpenPositionWithOrders();
                int x;
                while (true)
                {
                    List<double> orderId = await Simulate_Trading.GetOpenOrders("BTCUSDT");
                    x = orderId.Count;
                    if (x < 2)
                    {
                        if (orderId[0] == null)
                        {
                            await Simulate_Trading.CancelOrder("BTCUSDT", orderId[1]);
                            orderId.Remove(1);
                            //x--;
                        }
                        else
                        {
                            await Simulate_Trading.CancelOrder("BTCUSDT", orderId[0]);
                            orderId.Remove(0);
                            //x--;
                        }
                        break;
                    }
                    Thread.Sleep(5000);
                }
            }
        }
    }
}
