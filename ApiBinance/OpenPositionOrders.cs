using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiBinance
{
    class OpenPositionOrders
    {
        public static async Task<double[]> OpenPositionWithOrders()
        {
            //var order = await Simulate_Trading.BuySell("BTCUSDT", "BUY", "MARKET", "0.04");
            var order = await Simulate_Trading.BuySell("ETHUSDT", "BUY", "MARKET", "0.5");
            if (order == null)
            {
                Console.WriteLine("poshel nahui");
                return null;
            }

            var tasks = new Task<double>[]
            {
                    //Simulate_Trading.PlaceBuyOrderStopMarket("BTCUSDT", "SELL", "STOP_MARKET", "0.04", "37000"),
                    //Simulate_Trading.PlaceBuyOrderStopMarket2("BTCUSDT", "SELL", "TAKE_PROFIT_MARKET", "0.04", "37200")
                    Simulate_Trading.PlaceBuyOrderStopMarket("ETHUSDT", "SELL", "STOP_MARKET", "0.5", "2000"),
                    Simulate_Trading.PlaceBuyOrderStopMarket2("ETHUSDT", "SELL", "TAKE_PROFIT_MARKET", "0.5", "2070")
            };
            var results = await Task.WhenAll(tasks);
            return results;
        }

        public static async Task OpenPos(string symbol)
        {
            double[] check = await OpenPositionOrders.OpenPositionWithOrders();
            if(check == null)
            {
                return;
            }
            else
            {
                while (true)
                {
                    List<double> orderId = await Simulate_Trading.GetOpenOrders(symbol);
                    //List<long> orderId = await Simulate_Trading.GetOpenOrders(symbols);
                    Console.WriteLine(symbol);
                    if (orderId.Count < 2)
                    {
                        if (orderId[0] == null)
                        {
                            //await Simulate_Trading.CancelOrder(symbol, orderId[1]);
                            await Simulate_Trading.CancelOrder(symbol, orderId[1]);
                            orderId.Remove(1);
                        }
                        else
                        {
                            //await Simulate_Trading.CancelOrder(symbol, orderId[0]);
                            await Simulate_Trading.CancelOrder(symbol, orderId[0]);
                            orderId.Remove(0);
                        }
                        break;
                    }
                    Thread.Sleep(5000);
                }
            }
        }

        public static async Task CheckOpenPos(string symbol)
        {
            List<string> openPositions = await Simulate_Trading.TESTGetOpenPositionFutures();
            if (openPositions.Contains(symbol))
            {
                Console.WriteLine("sosi bibu");
            }
            else
            {
                await OpenPositionOrders.OpenPos(symbol);
            }
        }
    }
}
