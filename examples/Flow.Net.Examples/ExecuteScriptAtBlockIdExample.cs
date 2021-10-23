using Flow.Net.Examples.Utilities;
using Flow.Net.Sdk;
using System;
using System.Threading.Tasks;

namespace Flow.Net.Examples
{
    public class ExecuteScriptAtBlockIdExample : ExampleBase
    {
        public static async Task RunAsync()
        {
            await CreateFlowClientAsync();

            ColorConsole.WriteWrappedHeader("ExecuteScriptAtBlockIdAsync example", headerColor: ConsoleColor.Yellow, dashColor: ConsoleColor.Yellow);

            var script = "pub fun main(): Int { return 1 + 3 }";
            ColorConsole.WriteInfo($"\nscript:\n{script}");

            var latestBlock = await _flowClient.GetLatestBlockAsync(); // getting a height for example purpose
            var response = await _flowClient.ExecuteScriptAtBlockIdAsync(script.FromStringToByteString(), latestBlock.Id);

            ConvertToConsoleMessage.WriteSuccessMessage(response);

            ColorConsole.WriteWrappedHeader("End ExecuteScriptAtBlockIdAsync example", headerColor: ConsoleColor.Yellow, dashColor: ConsoleColor.Gray);
        }
    }
}
