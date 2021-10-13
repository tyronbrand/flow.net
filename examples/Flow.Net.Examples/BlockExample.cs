using Flow.Net.Sdk.Client;
using Flow.Net.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Flow.Net.Examples
{
    public class BlockExample
    {
        public static FlowClientAsync _flowClient;
        
        public static async Task RunAsync()
        {
            var networkUrl = "127.0.0.1:3569"; // emulator
            //var networkUrl = "access.devnet.nodes.onflow.org:9000"; // testnet

            _flowClient = FlowClientAsync.Create(networkUrl);

            await _flowClient.PingAsync();

            // examples
            await GetLatestBlockExample();
            await GetBlockByHeightExample();
            await GetBlockByIdExample();
            await GetLatestBlockHeaderExample();
            await GetBlockHeaderByHeightExample();
            await GetBlockHeaderByIdExample();
        }

        public static async Task GetLatestBlockExample()
        {
            var latestBlock = await _flowClient.GetLatestBlockAsync();

            Console.WriteLine($"{JsonConvert.SerializeObject(latestBlock)}\n");
        }

        public static async Task GetBlockByHeightExample()
        {
            var latestBlock = await _flowClient.GetLatestBlockAsync(); // getting a height for example purpose
            var blockByHeightResult = await _flowClient.GetBlockByHeightAsync(latestBlock.Height);

            Console.WriteLine($"{JsonConvert.SerializeObject(blockByHeightResult)}\n");
        }

        public static async Task GetBlockByIdExample()
        {
            var latestBlock = await _flowClient.GetLatestBlockAsync(); // getting an Id for example purpose
            var blockByIdResult = await _flowClient.GetBlockByIdAsync(latestBlock.Id);

            Console.WriteLine($"{JsonConvert.SerializeObject(blockByIdResult)}\n");
        }

        public static async Task GetLatestBlockHeaderExample()
        {
            var latestBlockHeader = await _flowClient.GetLatestBlockHeaderAsync();

            Console.WriteLine($"{JsonConvert.SerializeObject(latestBlockHeader)}\n");
        }

        public static async Task GetBlockHeaderByHeightExample()
        {
            var latestBlock = await _flowClient.GetLatestBlockAsync(); // getting a height for example purpose
            var blockByHeightResult = await _flowClient.GetBlockHeaderByHeightAsync(latestBlock.Height);

            Console.WriteLine($"{JsonConvert.SerializeObject(blockByHeightResult)}\n");
        }

        public static async Task GetBlockHeaderByIdExample()
        {
            var latestBlock = await _flowClient.GetLatestBlockAsync(); // getting an Id for example purpose
            var blockByIdResult = await _flowClient.GetBlockHeaderByIdAsync(latestBlock.Id);

            Console.WriteLine($"{JsonConvert.SerializeObject(blockByIdResult)}\n");
        }
    }
}
