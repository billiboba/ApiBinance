using ApiBinance.ApiByBit;
using CryptoExchange.Net.Authentication;
using Bybit.Net;
using Bybit.Net.Objects;
namespace ApiBinance
{
    public class Program
    {
        public static async Task Main()
        {
            BybitAccount account = new BybitAccount();
            account.PlaceOrder();
            account.GetWalletBalance();
        }
    }
}