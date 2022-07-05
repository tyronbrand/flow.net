using Flow.Net.Sdk.Core.Client;
using Flow.Net.Sdk.Core.Models;
using System;
using System.Threading.Tasks;

namespace Flow.Net.Examples.BlockExamples
{
    public class BlockExample : ExampleBase
    {
        public static async Task RunAsync(IFlowClient flowClient)
        {
            Console.WriteLine("\nRunning BlockExample\n");
            FlowClient = flowClient;
            await Demo();
            Console.WriteLine("\nBlockExample Complete\n");
        }

        private static async Task Demo()
        {
            // get the latest sealed block
            var latestBlock = await FlowClient.GetLatestBlockAsync();
            PrintResult(latestBlock);

            // get the block by ID
            var blockByIdResult = await FlowClient.GetBlockByIdAsync(latestBlock.Header.Id);
            PrintResult(blockByIdResult);

            // get block by height
            var blockByHeightResult = await FlowClient.GetBlockByHeightAsync(latestBlock.Header.Height);
            PrintResult(blockByHeightResult);
        }

        private static void PrintResult(FlowBlock flowBlock)
        {
            Console.WriteLine($"ID: {flowBlock.Header.Id}");
            Console.WriteLine($"height: {flowBlock.Header.Height}");
            Console.WriteLine($"timestamp: {flowBlock.Header.Timestamp}\n");            
        }
    }
}
