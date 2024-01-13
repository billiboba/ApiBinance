using ApiBinance.ApiByBit;
using CryptoExchange.Net.Authentication;
using Bybit.Net;
using Bybit.Net.Objects;
using Cryptocurrency.ApiByBit;
namespace ApiBinance
{
    public class Program
    {
        public static async Task Main()
        {
            //GetOrdersBybit.GetHistoryCurrency();
            //GetOrdersBybit.MarketPlaceOrder();
            //GetOrdersBybit.TakeProfitOrder();
            //
            //GetOrdersBybit.LimitPlaceOrder();
            //GetOrdersBybit.CancelOrder();
            //account.GetWalletBalance();
            //GetOrdersBybit.GetOpenPositions("OPUSDT");
            //GetOrdersBybit.GetOpenOrdersBybit("SPELLUSDT");
            //double boba = GetOrdersBybit.GetVolatily("ETHUSDT");
            //Console.WriteLine(boba);
            GetOrdersBybit.GetClosedPositions();
        }
    }
}