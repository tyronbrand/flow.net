using Flow.Net.Examples.Utilities;
using System;
using System.Threading.Tasks;

namespace Flow.Net.Examples
{
    public class GetBlockByIdExample : ExampleBase
    {
        public static async Task RunAsync()
        {
            await CreateFlowClientAsync();

            ColorConsole.WriteWrappedHeader("GetBlockByIdAsync example", headerColor: ConsoleColor.Yellow, dashColor: ConsoleColor.Yellow);

            var latestBlock = await _flowClient.GetLatestBlockAsync(); // getting an Id for example purpose
            var blockByIdResult = await _flowClient.GetBlockByIdAsync(latestBlock.Id);

            if (blockByIdResult != null)
                ConvertToConsoleMessage.WriteSuccessMessage(blockByIdResult);

            ColorConsole.WriteWrappedHeader("End GetBlockByIdAsync example", headerColor: ConsoleColor.Yellow, dashColor: ConsoleColor.Gray);
        }
    }
}
