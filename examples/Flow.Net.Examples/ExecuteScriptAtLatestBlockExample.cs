using Flow.Net.Examples.Utilities;
using Flow.Net.Sdk;
using Flow.Net.Sdk.Cadence;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Flow.Net.Examples
{
    public class ExecuteScriptAtLatestBlockExample : ExampleBase
    {
        public static async Task RunAsync()
        {
            await CreateFlowClientAsync();

            ColorConsole.WriteWrappedHeader("ExecuteScriptAtLatestBlockAsync example", headerColor: ConsoleColor.Yellow, dashColor: ConsoleColor.Yellow);

            var script = "pub fun main(): Int { return 1 + 1 }";
            ColorConsole.WriteInfo($"\nscript:\n{script}");

            var response = await _flowClient.ExecuteScriptAtLatestBlockAsync(script.FromStringToByteString());
            ConvertToConsoleMessage.WriteSuccessMessage(response);

            ColorConsole.WriteWrappedHeader("End ExecuteScriptAtLatestBlockAsync example", headerColor: ConsoleColor.Yellow, dashColor: ConsoleColor.Gray);
        }
    }
}
