using Flow.Net.Sdk;
using Flow.Net.Sdk.Cadence;
using Flow.Net.Sdk.Models;
using System;
using System.Threading.Tasks;

namespace Flow.Net.Examples.ScriptExamples
{
    public class ExecuteScriptAtBlockHeightExample : ExampleBase
    {
        public static async Task RunAsync()
        {
            Console.WriteLine("\nRunning ExecuteScriptAtBlockHeightExample\n");
            await CreateFlowClientAsync();
            await Demo();
            Console.WriteLine("\nExecuteScriptAtBlockHeightExample Complete\n");
        }

        private static async Task Demo()
        {
            var script = "pub fun main(): Int { return 1 + 2 }";
            var latestBlock = await FlowClient.GetLatestBlockAsync(); // getting a height for example purpose
            var response = await FlowClient.ExecuteScriptAtBlockHeightAsync(
                new FlowScript
                { 
                    Script = new FlowCadenceScript
                    {
                        Script = script
                    }
                }, latestBlock.Height);
            PrintResult(response);
        }

        private static void PrintResult(ICadence cadence)
        {
            Console.WriteLine($"Script result: {cadence.As<CadenceNumber>().Value}");
        }
    }
}
