using Flow.Net.Sdk;
using Flow.Net.Sdk.Models;
using System;
using System.Threading.Tasks;

namespace Flow.Net.Examples
{
    public class BlockHeaderExample : ExampleBase
    {
        public static async Task RunAsync()
        {
            Console.WriteLine("\nRunning BlockHeaderExample\n");
            await CreateFlowClientAsync();
            await Demo();
            Console.WriteLine("\nBlockHeaderExample Complete\n");
        }

        private static async Task Demo()
        {            
            // get the latest sealed block header
            var latestBlockHeader = await FlowClient.GetLatestBlockHeaderAsync();
            PrintResult(latestBlockHeader);

            // get the block header by ID
            var blockHeaderByIdResult = await FlowClient.GetBlockHeaderByIdAsync(latestBlockHeader.Id);
            PrintResult(blockHeaderByIdResult);

            // get block header by height
            var blockHeaderByHeightResult = await FlowClient.GetBlockHeaderByHeightAsync(latestBlockHeader.Height);
            PrintResult(blockHeaderByHeightResult);
        }

        private static void PrintResult(FlowBlockHeader flowBlockHeader)
        {
            Console.WriteLine($"ID: {flowBlockHeader.Id.FromByteStringToHex()}");
            Console.WriteLine($"height: {flowBlockHeader.Height}");
            Console.WriteLine($"timestamp: {flowBlockHeader.Timestamp}\n");
        }
    }
}
