using System.Threading.Tasks;

namespace Flow.Net.Examples
{
    internal static class Program
    {
        private static async Task Main()
        {
            // Block example
            await BlockExamples.BlockExample.RunAsync();
            await BlockExamples.BlockHeaderExample.RunAsync();

            // Event example
            await EventExamples.GetEventsExample.RunAsync();

            // Script example
            await ScriptExamples.ScriptExample.RunAsync();
            await ScriptExamples.ExecuteScriptAtLatestBlockWithParametersExample.RunAsync();
            await ScriptExamples.ExecuteScriptAtLatestBlockExample.RunAsync();
            await ScriptExamples.ExecuteScriptAtBlockHeightExample.RunAsync();
            await ScriptExamples.ExecuteScriptAtBlockIdExample.RunAsync();

            // Collection example
            await CollectionExamples.GetCollectionExample.RunAsync();

            // Transaction examples
            await TransactionExamples.GetTransactionExample.RunAsync();
            await TransactionExamples.SinglePartySingleSignatureExample.RunAsync();
            await TransactionExamples.SinglePartyMultiSignatureExample.RunAsync();
            await TransactionExamples.MultiPartySingleSignatureExample.RunAsync();
            await TransactionExamples.MultiPartyTwoAuthorizersExample.RunAsync();
            await TransactionExamples.MultiPartyMultiSignatureExample.RunAsync();

            // Account example            
            await AccountExamples.CreateAccountExample.RunAsync();
            await AccountExamples.CreateAccountWithContractExample.RunAsync();
            await AccountExamples.DeployUpdateDeleteContractExample.RunAsync();
            await AccountExamples.GetAccountExample.RunAsync();

            // User signature examples
            await UserSignaturesExamples.UserSignatureExample.RunAsync();
            await UserSignaturesExamples.UserSignatureValidateAnyExample.RunAsync();
            await UserSignaturesExamples.UserSignatureValidateAllExample.RunAsync();

        }
    }
}
