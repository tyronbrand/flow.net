using Flow.Net.Sdk;
using Flow.Net.Sdk.Cadence;
using Flow.Net.Sdk.Models;
using System;
using System.Threading.Tasks;

namespace Flow.Net.Examples.ScriptExamples
{
    public class ExecuteScriptAtBlockIdExample : ExampleBase
    {
        public static async Task RunAsync()
        {
            Console.WriteLine("\nRunning ExecuteScriptAtBlockIdExample\n");
            await CreateFlowClientAsync();
            await Demo();
            Console.WriteLine("\nExecuteScriptAtBlockIdExample Complete\n");
        }

        private static async Task Demo()
        {
            var script = "pub fun main(): Int { return 1 + 3 }";
            var latestBlock = await FlowClient.GetLatestBlockAsync(); // getting a height for example purpose
            var response = await FlowClient.ExecuteScriptAtBlockIdAsync(
                new FlowScript
                {
                    Script = script
                }, latestBlock.Id);
            PrintResult(response);
        }

        private static void PrintResult(ICadence cadence)
        {
            Console.WriteLine($"Script result: {cadence.As<CadenceNumber>().Value}");
        }
    }
}
