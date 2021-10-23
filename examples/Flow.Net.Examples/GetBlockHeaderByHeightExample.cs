using Flow.Net.Examples.Utilities;
using System;
using System.Threading.Tasks;

namespace Flow.Net.Examples
{
    public class GetBlockHeaderByHeightExample : ExampleBase
    {
        public static async Task RunAsync()
        {
            await CreateFlowClientAsync();

            ColorConsole.WriteWrappedHeader("GetBlockHeaderByHeightAsync example", headerColor: ConsoleColor.Yellow, dashColor: ConsoleColor.Yellow);

            var latestBlock = await _flowClient.GetLatestBlockAsync(); // getting a height for example purpose
            var blockHeaderByHeightResult = await _flowClient.GetBlockHeaderByHeightAsync(latestBlock.Height);

            if (blockHeaderByHeightResult != null)
                ConvertToConsoleMessage.WriteSuccessMessage(blockHeaderByHeightResult);

            ColorConsole.WriteWrappedHeader("End GetBlockHeaderByHeightAsync example", headerColor: ConsoleColor.Yellow, dashColor: ConsoleColor.Gray);
        }
    }
}
