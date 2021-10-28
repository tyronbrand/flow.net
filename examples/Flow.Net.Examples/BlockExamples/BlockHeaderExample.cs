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
            await CreateFlowClientAsync();
            await Demo();
        }

        private static async Task Demo()
        {            
            // get the latest sealed block header
            var latestBlockHeader = await _flowClient.GetLatestBlockHeaderAsync();
            PrintResult(latestBlockHeader);

            // get the block header by ID
            var blockHeaderByIdResult = await _flowClient.GetBlockHeaderByIdAsync(latestBlockHeader.Id);
            PrintResult(blockHeaderByIdResult);

            // get block header by height
            var blockHeaderByHeightResult = await _flowClient.GetBlockHeaderByHeightAsync(latestBlockHeader.Height);
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
