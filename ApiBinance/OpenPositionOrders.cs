using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiBinance
{
    class OpenPositionOrders
    {
        public static async Task<double[]> OpenPositionWithOrders(string symbol)
        {
            double price = await WebBinance.GetFuturesPrice(symbol); //Получение цены криптовалюты в данный момент
            double quan = await CheckQuantity(symbol); //Проверка сколько можем купить валюты

            double sl = price - price * 0.01; //Установка стоплосс
            double tp = price + price * 0.04; //Установка тейкпрофит

            double sl_ = Math.Round(sl,2); //Установка стоплосс
            double tp_ = Math.Round(tp, 2);
            var order = await Simulate_Trading.BuySell(symbol, "BUY", "MARKET", quan);
            if (order == null)
            {
                Console.WriteLine("poshel nahui");
                return null;
            }

            var tasks = new Task<double>[]
            {
                    Simulate_Trading.PlaceBuyOrderStopMarket(symbol, "SELL", "STOP_MARKET", quan, sl_),
                    Simulate_Trading.PlaceBuyOrderStopMarket2(symbol, "SELL", "TAKE_PROFIT_MARKET", quan, tp_)
            };
            var results = await Task.WhenAll(tasks);
            return results;
        }

        public static async Task OpenPos(string symbol)
        {
            //
            List<double> orderId1 = await Simulate_Trading.GetOpenOrders(symbol);
            if (orderId1.Count == 1)
            {
                if (orderId1[0] == null)
                {
                    await Simulate_Trading.CancelOrder(symbol, orderId1[1]);
                    orderId1.Remove(1);
                }
                else
                {
                    await Simulate_Trading.CancelOrder(symbol, orderId1[0]);
                    orderId1.Remove(0);
                }
            }
            ////
            else
            {
                double[] check = await OpenPositionOrders.OpenPositionWithOrders(symbol);
                if (check == null)
                {
                    return;
                }
                else
                {
                    List<double> orderId = await Simulate_Trading.GetOpenOrders(symbol);
                    if (orderId.Count < 2)
                    {
                        if (orderId[0] == null)
                        {
                            await Simulate_Trading.CancelOrder(symbol, orderId[1]);
                            orderId.Remove(1);
                        }
                        else
                        {
                            await Simulate_Trading.CancelOrder(symbol, orderId[0]);
                            orderId.Remove(0);
                        }
                    }
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

        public static async Task<double> CheckQuantity(string symbol)
        {
            double Balance = await WebBinance.GetAccountBalance();
            double price = await WebBinance.GetFuturesPrice(symbol);
            double quantity = (Balance / 10) / price;
            double quan2 = Math.Round(quantity, 2);
            //Console.WriteLine(" symbol quantity: " + quan2);
            return quan2;
        }
    }
}
