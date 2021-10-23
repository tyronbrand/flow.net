using Flow.Net.Examples.Utilities;
using System;
using System.Threading.Tasks;

namespace Flow.Net.Examples
{
    public class GetBlockHeaderByIdExample : ExampleBase
    {
        public static async Task RunAsync()
        {
            await CreateFlowClientAsync();

            ColorConsole.WriteWrappedHeader("GetBlockHeaderByIdAsync example", headerColor: ConsoleColor.Yellow, dashColor: ConsoleColor.Yellow);

            var latestBlock = await _flowClient.GetLatestBlockAsync(); // getting an Id for example purpose
            var blockHeaderByIdResult = await _flowClient.GetBlockHeaderByIdAsync(latestBlock.Id);

            if (blockHeaderByIdResult != null)
                ConvertToConsoleMessage.WriteSuccessMessage(blockHeaderByIdResult);

            ColorConsole.WriteWrappedHeader("End GetBlockHeaderByIdAsync example", headerColor: ConsoleColor.Yellow, dashColor: ConsoleColor.Gray);
        }
    }
}
