using Flow.Net.Examples.AccountExamples;
using Flow.Net.Examples.BlockExamples;
using Flow.Net.Examples.CollectionExamples;
using Flow.Net.Examples.EventExamples;
using Flow.Net.Examples.ScriptExamples;
using Flow.Net.Examples.TransactionExamples;
using Flow.Net.Examples.UserSignaturesExamples;
using System.Threading.Tasks;

namespace Flow.Net.Examples
{
    internal static class Program
    {
        private static async Task Main()
        {
            // Block example
            await BlockExample.RunAsync();
            await BlockHeaderExample.RunAsync();

            // Event example
            await GetEventsExample.RunAsync();

            // Script example
            await ScriptExample.RunAsync();
            await ExecuteScriptAtLatestBlockWithParametersExample.RunAsync();
            await ExecuteScriptAtLatestBlockExample.RunAsync();
            await ExecuteScriptAtBlockHeightExample.RunAsync();
            await ExecuteScriptAtBlockIdExample.RunAsync();

            // Collection example
            await GetCollectionExample.RunAsync();

            // Transaction examples
            await GetTransactionExample.RunAsync();
            await SinglePartySingleSignatureExample.RunAsync();
            await SinglePartyMultiSignatureExample.RunAsync();
            await MultiPartySingleSignatureExample.RunAsync();
            await MultiPartyTwoAuthorizersExample.RunAsync();
            await MultiPartyMultiSignatureExample.RunAsync();

            // Account example            
            await CreateAccountExample.RunAsync();
            await CreateAccountWithContractExample.RunAsync();
            await DeployUpdateDeleteContractExample.RunAsync();
            await GetAccountExample.RunAsync();

            // User signature examples
            await UserSignatureExample.RunAsync();
            await UserSignatureValidateAnyExample.RunAsync();
            await UserSignatureValidateAllExample.RunAsync();
        }
    }
}
