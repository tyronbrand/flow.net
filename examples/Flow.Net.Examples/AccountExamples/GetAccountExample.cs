using Flow.Net.Sdk;
using Flow.Net.Sdk.Models;
using System;
using System.Threading.Tasks;

namespace Flow.Net.Examples
{
    public class GetAccountExample : ExampleBase
    {
        public static async Task RunAsync()
        {
            Console.WriteLine("\nRunning GetAccountExample\n");
            await CreateFlowClientAsync();
            await Demo();
            Console.WriteLine("\nGetAccountExample Complete\n");
        }

        private static async Task Demo()
        {
            // get account from the latest block
            var address = new FlowAddress("f8d6e0586b0a20c7");
            var account = await FlowClient.GetAccountAtLatestBlockAsync(address);
            PrintResult(account);

            // get account from the block by height 0
            account = await FlowClient.GetAccountAtBlockHeightAsync(address, 0);
            PrintResult(account);
        }

        private static void PrintResult(FlowAccount flowAccount)
        {
            Console.WriteLine($"Address: {flowAccount.Address.HexValue}");
            Console.WriteLine($"Balance: {flowAccount.Balance}");
            Console.WriteLine($"Contracts: {flowAccount.Contracts.Count}");
            Console.WriteLine($"Keys: {flowAccount.Keys.Count}\n");
        }
    }
}
