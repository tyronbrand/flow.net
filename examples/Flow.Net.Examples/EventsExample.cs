using Flow.Net.Sdk;
using Flow.Net.Sdk.Client;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Flow.Net.Examples
{
    public class EventsExample
    {
        public static FlowClientAsync _flowClient;

        public static async Task RunAsync()
        {
            //var networkUrl = "127.0.0.1:3569"; // emulator
            var networkUrl = "access.devnet.nodes.onflow.org:9000"; // testnet

            _flowClient = FlowClientAsync.Create(networkUrl);

            await _flowClient.PingAsync();

            // examples
            await GetEventsForHeightRangeExample();
            await GetEventsForBlockIdsExample();
        }

        public static async Task GetEventsForHeightRangeExample()
        {
            // getting a height for example purpose
            var latestBlock = await _flowClient.GetLatestBlockAsync();
            ulong startHeight = 0;
            if (latestBlock.Height > 200)
                startHeight = latestBlock.Height - 200;

            var response = await _flowClient.GetEventsForHeightRangeAsync("flow.AccountCreated", startHeight, latestBlock.Height);

            Console.WriteLine(JsonConvert.SerializeObject(response, Formatting.Indented));
        }

        public static async Task GetEventsForBlockIdsExample()
        {
            // getting block ids for example purpose
            var latestBlock = await _flowClient.GetLatestBlockAsync();
            ulong startHeight = 0;
            if (latestBlock.Height > 200)
                startHeight = latestBlock.Height - 200;

            var events = await _flowClient.GetEventsForHeightRangeAsync("flow.AccountCreated", startHeight, latestBlock.Height);
            var distinctBlockIds = events.Select(p => p.BlockId).Distinct().TakeLast(50);

            var response = await _flowClient.GetEventsForBlockIdsAsync("flow.AccountCreated", distinctBlockIds);

            Console.WriteLine(JsonConvert.SerializeObject(response, Formatting.Indented));
        }
    }
}
