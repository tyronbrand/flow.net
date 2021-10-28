using Flow.Net.Sdk;
using System.Threading.Tasks;

namespace Flow.Net.Examples
{
    public class ExecuteScriptAtBlockHeightExample : ExampleBase
    {
        public static async Task RunAsync()
        {
            await CreateFlowClientAsync();
            await Demo();
        }

        private static async Task Demo()
        {
            var script = "pub fun main(): Int { return 1 + 2 }";
            var latestBlock = await _flowClient.GetLatestBlockAsync(); // getting a height for example purpose
            await _flowClient.ExecuteScriptAtBlockHeightAsync(script.FromStringToByteString(), latestBlock.Height);
        }
    }
}
