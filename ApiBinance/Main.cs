using ApiBinance;
namespace BinanceFuturesAccount

{
    public class Program : Transaction
    { 
        public static async Task Main()
        {
            var balance = await GetAccountInfo();
            Console.WriteLine(balance);

            while (true)
            {
            string symbol = Console.ReadLine();
            string side = Console.ReadLine();
            string type = Console.ReadLine();
            string quantity = Console.ReadLine();
            await BuySell(symbol, side, type, quantity);
            
                await GetOpenPositionFutures();
                Thread.Sleep(1000);
            }
            
        }
    }
}