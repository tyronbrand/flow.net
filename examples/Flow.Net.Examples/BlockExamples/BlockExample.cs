using Flow.Net.Sdk;
using Flow.Net.Sdk.Models;
using System;
using System.Threading.Tasks;

namespace Flow.Net.Examples
{
    public class BlockExample : ExampleBase
    {
        public static async Task RunAsync()
        {
            await CreateFlowClientAsync();
            await Demo();
        }

        private static async Task Demo()
        {
            // get the latest sealed block
            var latestBlock = await _flowClient.GetLatestBlockAsync();
            PrintResult(latestBlock);

            // get the block by ID
            var blockByIdResult = await _flowClient.GetBlockByIdAsync(latestBlock.Id);
            PrintResult(blockByIdResult);

            // get block by height
            var blockByHeightResult = await _flowClient.GetBlockByHeightAsync(latestBlock.Height);
            PrintResult(blockByHeightResult);
        }

        private static void PrintResult(FlowBlock flowBlock)
        {
            Console.WriteLine($"ID: {flowBlock.Id.FromByteStringToHex()}");
            Console.WriteLine($"height: {flowBlock.Height}");
            Console.WriteLine($"timestamp: {flowBlock.Timestamp}\n");            
        }
    }
}
