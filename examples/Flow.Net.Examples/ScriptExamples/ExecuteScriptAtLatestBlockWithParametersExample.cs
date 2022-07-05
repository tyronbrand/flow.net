using Flow.Net.Sdk.Core.Cadence;
using Flow.Net.Sdk.Core.Client;
using Flow.Net.Sdk.Core.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Flow.Net.Examples.ScriptExamples
{
    public class ExecuteScriptAtLatestBlockWithParametersExample : ExampleBase
    {
        public static async Task RunAsync(IFlowClient flowClient)
        {
            Console.WriteLine("\nRunning ExecuteScriptAtLatestBlockWithParametersExample\n");
            FlowClient = flowClient;
            await Demo();
            Console.WriteLine("\nExecuteScriptAtLatestBlockWithParametersExample Complete\n");
        }

        private static async Task Demo()
        {
            var script = "pub fun main(num: Int32): Int32 { return 54534 + num }";

            var arguments = new List<ICadence>
            {
                new CadenceNumber(CadenceNumberType.Int32, "834534")
            };

            var response = await FlowClient.ExecuteScriptAtLatestBlockAsync(
                new FlowScript
                {
                    Script = script,
                    Arguments = arguments
                });
            PrintResult(response);
        }

        private static void PrintResult(ICadence cadence)
        {
            Console.WriteLine($"Script result: {cadence.As<CadenceNumber>().Value}");
        }
    }
}
