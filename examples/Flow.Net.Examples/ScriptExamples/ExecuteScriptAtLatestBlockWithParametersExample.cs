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
            await Demo();
        }

        private static async Task Demo()
        {            
            var script = "pub fun main(num: Int32): Int32 { return 54534 + num }";
            
            var arguments = new List<ICadence>
            {
                new CadenceNumber(CadenceNumberType.Int32, "834534")
            };

            var response = await _flowClient.ExecuteScriptAtLatestBlockAsync(script.FromStringToByteString(), arguments);
            PrintResult(response);
        }

        private static void PrintResult(ICadence cadence)
        {
            Console.WriteLine($"Script result: {cadence.AsCadenceType<CadenceNumber>().Value}");
        }
    }
}
