using Flow.Net.Examples.Utilities;
using System;
using System.Threading.Tasks;

namespace Flow.Net.Examples
{
    public class GetLatestBlockHeaderExample : ExampleBase
    {
        public static async Task RunAsync()
        {
            await CreateFlowClientAsync();

            ColorConsole.WriteWrappedHeader("GetLatestBlockHeaderAsync example", headerColor: ConsoleColor.Yellow, dashColor: ConsoleColor.Yellow);

            var latestBlockHeader = await _flowClient.GetLatestBlockHeaderAsync();

            if (latestBlockHeader != null)
                ConvertToConsoleMessage.WriteSuccessMessage(latestBlockHeader);

            ColorConsole.WriteWrappedHeader("End GetLatestBlockHeaderAsync example", headerColor: ConsoleColor.Yellow, dashColor: ConsoleColor.Gray);
        }
    }
}
