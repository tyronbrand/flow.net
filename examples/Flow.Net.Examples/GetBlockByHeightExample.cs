using Flow.Net.Examples.Utilities;
using System;
using System.Threading.Tasks;

namespace Flow.Net.Examples
{
    public class GetBlockByHeightExample : ExampleBase
    {
        public static async Task RunAsync()
        {
            await CreateFlowClientAsync();

            ColorConsole.WriteWrappedHeader("GetBlockByHeightAsync example", headerColor: ConsoleColor.Yellow, dashColor: ConsoleColor.Yellow);

            var latestBlock = await _flowClient.GetLatestBlockAsync(); // getting a height for example purpose
            var blockByHeightResult = await _flowClient.GetBlockByHeightAsync(latestBlock.Height);

            if (blockByHeightResult != null)
                ConvertToConsoleMessage.WriteSuccessMessage(blockByHeightResult);

            ColorConsole.WriteWrappedHeader("End GetBlockByHeightAsync example", headerColor: ConsoleColor.Yellow, dashColor: ConsoleColor.Gray);
        }
    }
}
