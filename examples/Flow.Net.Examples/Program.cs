using Flow.Net.Sdk.Client.Grpc;
using Flow.Net.Sdk.Client.Http;
using Flow.Net.Sdk.Core.Client;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Flow.Net.Examples
{
    internal static class Program
    {
        private static async Task Main()
        {
            var flowClients = new List<IFlowClient>
            {
                // emulator addresses
                new FlowHttpClient(new HttpClient(), new FlowClientOptions { ServerUrl = Sdk.Client.Http.ServerUrl.EmulatorHost }),
                new FlowGrpcClient(new FlowGrpcClientOptions { ServerUrl = Sdk.Client.Grpc.ServerUrl.EmulatorHost })
            };

            foreach (var client in flowClients)
            {
                // Block example
                await BlockExamples.BlockExample.RunAsync(client);
                await BlockExamples.BlockHeaderExample.RunAsync(client);

                // Event example
                await EventExamples.GetEventsExample.RunAsync(client);

                // Script example
                await ScriptExamples.ScriptExample.RunAsync(client);
                await ScriptExamples.ExecuteScriptAtLatestBlockWithParametersExample.RunAsync(client);
                await ScriptExamples.ExecuteScriptAtLatestBlockExample.RunAsync(client);
                await ScriptExamples.ExecuteScriptAtBlockHeightExample.RunAsync(client);
                await ScriptExamples.ExecuteScriptAtBlockIdExample.RunAsync(client);

                // Collection example
                await CollectionExamples.GetCollectionExample.RunAsync(client);

                // Transaction examples
                await TransactionExamples.GetTransactionExample.RunAsync(client);
                await TransactionExamples.SinglePartySingleSignatureExample.RunAsync(client);
                await TransactionExamples.SinglePartyMultiSignatureExample.RunAsync(client);
                await TransactionExamples.MultiPartySingleSignatureExample.RunAsync(client);
                await TransactionExamples.MultiPartyTwoAuthorizersExample.RunAsync(client);
                await TransactionExamples.MultiPartyMultiSignatureExample.RunAsync(client);

                // Account example            
                await AccountExamples.CreateAccountExample.RunAsync(client);
                await AccountExamples.CreateAccountWithContractExample.RunAsync(client);
                await AccountExamples.DeployUpdateDeleteContractExample.RunAsync(client);
                await AccountExamples.GetAccountExample.RunAsync(client);

                // User signature examples
                await UserSignaturesExamples.UserSignatureExample.RunAsync(client);
                await UserSignaturesExamples.UserSignatureValidateAnyExample.RunAsync(client);
                await UserSignaturesExamples.UserSignatureValidateAllExample.RunAsync(client);

                // Storage examples
                await StorageExamples.StorageUsageExample.RunAsync(client);
            }
        }
    }
}
