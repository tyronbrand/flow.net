using Flow.Net.Examples.Utilities;
using Flow.Net.Sdk;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Flow.Net.Examples
{
    public class GetEventsForBlockIdsExample : ExampleBase
    {
        public static async Task RunAsync()
        {
            await CreateFlowClientAsync();

            ColorConsole.WriteWrappedHeader("GetEventsForBlockIdsAsync example", headerColor: ConsoleColor.Yellow, dashColor: ConsoleColor.Yellow);

            // getting block ids for example purpose
            var latestBlock = await _flowClient.GetLatestBlockAsync();
            ulong startHeight = 0;
            if (latestBlock.Height > 200)
                startHeight = latestBlock.Height - 200;

            var events = await _flowClient.GetEventsForHeightRangeAsync("flow.AccountCreated", startHeight, latestBlock.Height);
            var distinctBlockIds = events.Select(p => p.BlockId).Distinct().TakeLast(10);

            ColorConsole.WriteWrappedSubHeader($"Getting \"flow.AccountCreated\" events.\nblock ids:\n{string.Join(",\n", distinctBlockIds.Select(s => s.FromByteStringToHex()).ToList())}");

            var response = await _flowClient.GetEventsForBlockIdsAsync("flow.AccountCreated", distinctBlockIds);

            ConvertToConsoleMessage.WriteSuccessMessage(response);

            ColorConsole.WriteWrappedHeader("End GetEventsForBlockIdsAsync example", headerColor: ConsoleColor.Yellow, dashColor: ConsoleColor.Gray);       
        }
    }
}
