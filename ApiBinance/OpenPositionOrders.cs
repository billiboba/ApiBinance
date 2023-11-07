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
            //await Simulate_Trading.BuySell("BTCUSDT", "BUY", "MARKET", "0.04");
            var order = await Simulate_Trading.BuySell("BTCUSDT", "BUY", "MARKET", "0.04");
            double[] results = new double[2];
            if (order == null)
            {
                Console.WriteLine("poshel nahui");
            }
            else
            {
                //var boba = Task.WhenAll(Simulate_Trading.PlaceBuyOrderStopMarket("BTCUSDT", "SELL", "STOP_MARKET", "0.04", "34000"), Simulate_Trading.PlaceBuyOrderStopMarket2("BTCUSDT", "SELL", "TAKE_PROFIT_MARKET", "0.04", "36000"));
                Task<double>[] tasks = new Task<double>[]
                {
                    Simulate_Trading.PlaceBuyOrderStopMarket("BTCUSDT", "SELL", "STOP_MARKET", "0.04", "35000"),
                    Simulate_Trading.PlaceBuyOrderStopMarket2("BTCUSDT", "SELL", "TAKE_PROFIT_MARKET", "0.04", "35200")
                };

                await Task.WhenAll(tasks);

                results = new double[tasks.Length];
                for (int i = 0; i < tasks.Length; i++)
                {
                    results[i] = tasks[i].Result;
                }
            }
            return results;
        }
    }
}
