using Flow.Net.Sdk.Client.Http;
using Flow.Net.Sdk.Core;
using Flow.Net.Sdk.Core.Models;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Flow.Net.Examples
{
    public abstract class HttpExampleBase
    {
        protected static FlowHttpClient FlowClient { get; private set; }

        protected static async Task<FlowHttpClient> CreateFlowClientAsync()
        {
            const string networkUrl = "http://127.0.0.1:8888/v1"; // emulator

            if (FlowClient != null)
                return FlowClient;

            FlowClient = new FlowHttpClient(new HttpClient(), networkUrl);
            await FlowClient.PingAsync();

            return FlowClient;
        }

        //protected static async Task<FlowAccount> CreateAccountAsync(IList<FlowAccountKey> newFlowAccountKeys)
        //{
        //    var response = await CreateAddressTransaction(newFlowAccountKeys);

        //    var newAccountAddress = response.Events.AccountCreatedAddress();

        //    // get new account details
        //    var newAccount = await FlowClient.GetAccountAtLatestBlockAsync(newAccountAddress);
        //    newAccount.Keys = FlowAccountKey.UpdateFlowAccountKeys(newFlowAccountKeys, newAccount.Keys);
        //    return newAccount;
        //}

        protected static async Task<string> RandomTransactionAsync()
        {
            // read flow.json
            var config = Utilities.ReadConfig();
            // get account from config
            var accountConfig = config.Accounts["emulator-account"];
            // get service account at latest block
            var serviceAccount = await FlowClient.GetAccountAtLatestBlockAsync(accountConfig.Address);
            // we can create a Signer with the serviceAccount and the accountConfig
            serviceAccount = Utilities.AddSignerFromConfigAccount(accountConfig, serviceAccount);

            // service key to use
            var serviceAccountKey = serviceAccount.Keys.FirstOrDefault();

            var latestBlock = await FlowClient.GetLatestBlockAsync();
            var tx = new FlowTransaction
            {
                Script = "transaction {}",
                ReferenceBlockId = latestBlock.Header.Id,
                Payer = serviceAccount.Address,
                ProposalKey = new FlowProposalKey
                {
                    Address = serviceAccount.Address,
                    KeyId = serviceAccountKey.Index,
                    SequenceNumber = serviceAccountKey.SequenceNumber
                }
            };

            tx = FlowTransaction.AddEnvelopeSignature(tx, serviceAccount.Address, serviceAccountKey.Index, serviceAccountKey.Signer);

            var response = await FlowClient.SendTransactionAsync(tx);
            return response.Id;
        }

        //protected static async Task<FlowAddress> GenerateRandomAddressAsync()
        //{
        //    // generate random key
        //    var flowAccountKey = FlowAccountKey.GenerateRandomEcdsaKey(SignatureAlgo.ECDSA_P256, HashAlgo.SHA3_256);

        //    var response = await CreateAddressTransaction(new List<FlowAccountKey> { flowAccountKey });
        //    return response.Events.AccountCreatedAddress();
        //}

        //private static async Task<FlowTransactionResult> CreateAddressTransaction(IEnumerable<FlowAccountKey> newFlowAccountKeys)
        //{
        //    await CreateFlowClientAsync();

        //    // creator (typically a service account)
        //    var creatorAccount = await FlowClient.ReadAccountFromConfigAsync("emulator-account");
        //    // creator key to use
        //    var creatorAccountKey = creatorAccount.Keys.FirstOrDefault();

        //    // use template to create a transaction
        //    var tx = Account.CreateAccount(newFlowAccountKeys, creatorAccount.Address);

        //    // set the transaction payer and proposal key
        //    tx.Payer = creatorAccount.Address;
        //    tx.ProposalKey = new FlowProposalKey
        //    {
        //        Address = creatorAccount.Address,
        //        KeyId = creatorAccountKey.Index,
        //        SequenceNumber = creatorAccountKey.SequenceNumber
        //    };

        //    // get the latest sealed block to use as a reference block
        //    var latestBlock = await FlowClient.GetLatestBlockAsync();
        //    tx.ReferenceBlockId = latestBlock.Id;

        //    // sign and submit the transaction
        //    tx = FlowTransaction.AddEnvelopeSignature(tx, creatorAccount.Address, creatorAccountKey.Index, creatorAccountKey.Signer);

        //    var response = await FlowClient.SendTransactionAsync(tx);

        //    // wait for seal
        //    var sealedResponse = await FlowClient.WaitForSealAsync(response);

        //    if (sealedResponse.Status != Sdk.Protos.entities.TransactionStatus.Sealed)
        //        return null;

        //    return sealedResponse;
        //}
    }
}

