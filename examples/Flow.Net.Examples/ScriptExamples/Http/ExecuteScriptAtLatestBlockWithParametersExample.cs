using Flow.Net.Sdk.Client.Http;
using Flow.Net.Sdk.Core.Cadence;
using Flow.Net.Sdk.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Flow.Net.Examples.ScriptExamples.Http
{
    public class ExecuteScriptAtLatestBlockWithParametersExample
    { 
        public static async Task RunAsync()
        {
            Console.WriteLine("\nRunning ExecuteScriptAtLatestBlockWithParametersExample\n");            
            await Demo();
            Console.WriteLine("\nExecuteScriptAtLatestBlockWithParametersExample Complete\n");
        }

        private static async Task Demo()
        {
            using var httpClient = new HttpClient();
            var client = new FlowHttpClient(httpClient, "https://rest-testnet.onflow.org/v1");
            var script = "pub fun main(num: Int32): Int32 { return 54534 + num }";

            var arguments = new List<ICadence>
                {
                    new CadenceNumber(CadenceNumberType.Int32, "834534")
                };

            await client.ExecuteScriptAtLatestBlockAsync(
                new FlowScript
                {
                    Script = script,
                    Arguments = arguments
                });

            //PrintResult(response);
        }

        private static void PrintResult(ICadence cadence)
        {
            Console.WriteLine($"Script result: {cadence.As<CadenceNumber>().Value}");
        }
    }
}
