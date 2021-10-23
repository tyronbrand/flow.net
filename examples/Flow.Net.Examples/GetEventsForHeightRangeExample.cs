using Flow.Net.Examples.Utilities;
using System;
using System.Threading.Tasks;

namespace Flow.Net.Examples
{
    public class GetEventsForHeightRangeExample : ExampleBase
    {
        public static async Task RunAsync()
        {
            await CreateFlowClientAsync();

            ColorConsole.WriteWrappedHeader("GetEventsForHeightRangeAsync example", headerColor: ConsoleColor.Yellow, dashColor: ConsoleColor.Yellow);

            // getting a height for example purpose
            var latestBlock = await _flowClient.GetLatestBlockAsync();
            ulong startHeight = 0;
            if (latestBlock.Height > 200)
                startHeight = latestBlock.Height - 200;

            ColorConsole.WriteWrappedSubHeader($"Getting \"flow.AccountCreated\" events.\nstart height: {startHeight}\nend height: {latestBlock.Height}");
            var response = await _flowClient.GetEventsForHeightRangeAsync("flow.AccountCreated", startHeight, latestBlock.Height);

            ConvertToConsoleMessage.WriteSuccessMessage(response);

            ColorConsole.WriteWrappedHeader("End GetEventsForHeightRangeAsync example", headerColor: ConsoleColor.Yellow, dashColor: ConsoleColor.Gray);       
        }
    }
}
