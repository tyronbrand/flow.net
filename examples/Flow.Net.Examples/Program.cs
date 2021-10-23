using System.Threading.Tasks;

namespace Flow.Net.Examples
{
    class Program
    {
        static async Task Main(string[] args)
        {
            // Uncomment one or more of the examples below

            // Block examples
            //await GetLatestBlockExample.RunAsync();
            //await GetBlockByHeightExample.RunAsync();
            //await GetBlockByIdExample.RunAsync();
            //await GetLatestBlockHeaderExample.RunAsync();
            //await GetBlockHeaderByHeightExample.RunAsync();
            //await GetBlockHeaderByIdExample.RunAsync();

            // Account examples
            //await CreateAccountExample.RunAsync();
            //await CreateAccountWithContractExample.RunAsync();
            //await DeployUpdateDeleteContractExample.RunAsync();

            // Event examples
            //await GetEventsForHeightRangeExample.RunAsync();
            //await GetEventsForBlockIdsExample.RunAsync();

            // Script examples
            //await ExecuteScriptAtLatestBlockWithParametersExample.RunAsync();
            //await ExecuteScriptAtLatestBlockExample.RunAsync();
            //await ExecuteScriptAtBlockHeightExample.RunAsync();
            //await ExecuteScriptAtBlockIdExample.RunAsync();

            // Transaction examples
            await SinglePartySingleSignatureExample.RunAsync();
            //await SinglePartyMultiSignatureExample.RunAsync();
            //await MultiPartySingleSignatureExample.RunAsync();
            //await MultiPartyTwoAuthorizersExample.RunAsync();
            //await MultiPartyMultiSignatureExample.RunAsync();
        }
    }
}
