using Flow.Net.Examples.Utilities;
using System;
using System.Threading.Tasks;

namespace Flow.Net.Examples
{
    public class GetLatestBlockExample : ExampleBase
    {
        public static async Task RunAsync()
        {
            await CreateFlowClientAsync();

            ColorConsole.WriteWrappedHeader("GetLatestBlockAsync example", headerColor: ConsoleColor.Yellow, dashColor: ConsoleColor.Yellow);

            var latestBlock = await _flowClient.GetLatestBlockAsync();

            if (latestBlock != null)
                ConvertToConsoleMessage.WriteSuccessMessage(latestBlock);

            ColorConsole.WriteWrappedHeader("End GetLatestBlockAsync example", headerColor: ConsoleColor.Yellow, dashColor: ConsoleColor.Gray);
        }
    }
}
