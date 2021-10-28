using Flow.Net.Sdk;
using System.Threading.Tasks;

namespace Flow.Net.Examples
{
    public class ExecuteScriptAtLatestBlockExample : ExampleBase
    {
        public static async Task RunAsync()
        {
            await CreateFlowClientAsync();
            await Demo();
        }

        private static async Task Demo()
        {
            var script = "pub fun main(): Int { return 1 + 1 }";
            var response = await _flowClient.ExecuteScriptAtLatestBlockAsync(script.FromStringToByteString());            
        }
    }
}
