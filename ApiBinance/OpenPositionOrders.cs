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
            //var order = await Simulate_Trading.BuySell("ETHUSDT", "BUY", "MARKET", "0.5");
            if (order == null)
            {
                Console.WriteLine("poshel nahui");
                return null;
            }

            var tasks = new Task<double>[]
            {
                    Simulate_Trading.PlaceBuyOrderStopMarket("BTCUSDT", "SELL", "STOP_MARKET", "0.04", "35000"),
                    Simulate_Trading.PlaceBuyOrderStopMarket2("BTCUSDT", "SELL", "TAKE_PROFIT_MARKET", "0.04", "36300")
                    //Simulate_Trading.PlaceBuyOrderStopMarket("ETHUSDT", "SELL", "STOP_MARKET", "0.5", "1600"),
                    //Simulate_Trading.PlaceBuyOrderStopMarket2("ETHUSDT", "SELL", "TAKE_PROFIT_MARKET", "0.5", "2000")
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
                    //List<double> orderId = await Simulate_Trading.GetOpenOrders("ETHUSDT");
                    x = orderId.Count;
                    if (x < 2)
                    {
                        if (orderId[0] == null)
                        {
                            await Simulate_Trading.CancelOrder("BTCUSDT", orderId[1]);
                            //await Simulate_Trading.CancelOrder("ETHUSDT", orderId[1]);
                            orderId.Remove(1);
                            x--;
                        }
                        else
                        {
                            await Simulate_Trading.CancelOrder("BTCUSDT", orderId[0]);
                            //await Simulate_Trading.CancelOrder("ETHUSDT", orderId[0]);
                            orderId.Remove(0);
                            x--;
                        }
                        break;
                    }
                    Thread.Sleep(5000);
                }
            }
        }

        public static async Task CheckOpenPos(string symbols)
        {
            while (true)
            {
                List<string> boba = await Simulate_Trading.TESTGetOpenPositionFutures();
                if (boba.Count != 0)
                {
                    //foreach (string s in boba)
                    //{
                    //    if (symbols.Contains(s))
                    //    {
                    //        //continue;
                    //        Console.WriteLine("sosi");
                    //    }
                    //    else
                    //    {
                    //        await OpenPositionOrders.OpenPos();
                    //    }
                    //}
                    if (boba.Contains(symbols))
                    {
                        continue;
                        Console.WriteLine("sosi");
                        Console.WriteLine("asd" + boba.Count);
                    }
                    else
                    {
                        await OpenPositionOrders.OpenPos();
                    }
                }
                else
                {
                    await OpenPositionOrders.OpenPos();
                }
                //await OpenPositionOrders.OpenPos();



                Thread.Sleep(5000);
            };
        }
    }
}
