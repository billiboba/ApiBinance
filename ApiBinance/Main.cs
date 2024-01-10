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
            BybitAccount account = new BybitAccount();
            GetOrdersBybit.MarketPlaceOrder();
            GetOrdersBybit.TakeProfitOrder();
            //
            //GetOrdersBybit.LimitPlaceOrder();
            //GetOrdersBybit.CancelOrder();
            //account.GetWalletBalance();
            //GetOrdersBybit.GetOpenPositions();
            //GetOrdersBybit.GetOpenOrdersBybit("SPELLUSDT");
            //GetOrdersBybit.GetFuturesSimbolsBybit();
        }
    }
}