using System.Threading.Tasks;

namespace Flow.Net.Examples
{
    class Program
    {
        static async Task Main(string[] args)
        {
            // uncomment one or more of the examples below

            // Block example
            await BlockExample.RunAsync();
            //await BlockHeaderExample.RunAsync();

            // Event example
            //await GetEventsExample.RunAsync();

            // Script example
            //await ScriptExample.RunAsync();
            //await ExecuteScriptAtLatestBlockWithParametersExample.RunAsync();
            //await ExecuteScriptAtLatestBlockExample.RunAsync();
            //await ExecuteScriptAtBlockHeightExample.RunAsync();
            //await ExecuteScriptAtBlockIdExample.RunAsync();

            // Collection example
            //await GetCollectionExample.RunAsync();

            // Transaction examples
            //await GetTransactionExample.RunAsync();
            //await SinglePartySingleSignatureExample.RunAsync();
            //await SinglePartyMultiSignatureExample.RunAsync();
            //await MultiPartySingleSignatureExample.RunAsync();
            //await MultiPartyTwoAuthorizersExample.RunAsync();
            //await MultiPartyMultiSignatureExample.RunAsync();

            // Account example            
            //await CreateAccountExample.RunAsync();
            //await CreateAccountWithContractExample.RunAsync();
            //await DeployUpdateDeleteContractExample.RunAsync();
            //await GetAccountExample.RunAsync();
        }
    }
}
