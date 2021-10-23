using Flow.Net.Examples.Utilities;
using Flow.Net.Sdk;
using Flow.Net.Sdk.Cadence;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Flow.Net.Examples
{
    public class ExecuteScriptAtLatestBlockWithParametersExample : ExampleBase
    {
        public static async Task RunAsync()
        {
            await CreateFlowClientAsync();

            ColorConsole.WriteWrappedHeader("ExecuteScriptAtLatestBlockAsync with parameters example", headerColor: ConsoleColor.Yellow, dashColor: ConsoleColor.Yellow);

            var script = "pub fun main(num: Int32): Int32 { return 54534 + num }";
            ColorConsole.WriteInfo($"\nscript:\n{script}");

            var arguments = new List<ICadence>
            {
                new CadenceNumber(CadenceNumberType.Int32, "834534")
            };
            ConvertToConsoleMessage.WriteInfoMessage(arguments);

            var response = await _flowClient.ExecuteScriptAtLatestBlockAsync(script.FromStringToByteString(), arguments);
            ConvertToConsoleMessage.WriteSuccessMessage(response);

            ColorConsole.WriteWrappedHeader("End ExecuteScriptAtLatestBlockAsync with parameters example", headerColor: ConsoleColor.Yellow, dashColor: ConsoleColor.Gray);
        }
    }
}
