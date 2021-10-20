using Flow.Net.Sdk;
using Flow.Net.Sdk.Client;
using Flow.Net.Sdk.Templates;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Flow.Net.Examples
{
    public class CreateAccountWithContractExample
    {
        public static readonly string networkUrl = "127.0.0.1:3569"; // emulator

        public static async Task RunAsync()
        {
            // create a flow client
            var _flowClient = FlowClientAsync.Create(networkUrl);

            // creator (typically a service account)
            var creatorAccount = await _flowClient.ReadAccountFromConfigAsync("emulator-account");

            // generate our new account key
            var flowAccountKey = FlowAccountKey.NewEcdsaAccountKey(SignatureAlgo.ECDSA_secp256k1, HashAlgo.SHA3_256, 1000);

            // contract to deploy
            var helloWorldContract = Utilities.ReadCadenceScript("hello-world-contract");
            var flowContract = new FlowContract
            {
                Name = "HelloWorld",
                Source = helloWorldContract
            };

            // use template to create a transaction
            var tx = Account.CreateAccount(new List<FlowAccountKey> { flowAccountKey }, creatorAccount.Address, new List<FlowContract> { flowContract });

            // creator key to use
            var creatorAccountKey = creatorAccount.Keys.FirstOrDefault();

            // set the transaction payer and proposal key
            tx.Payer = creatorAccount.Address;
            tx.ProposalKey = new FlowProposalKey
            {
                Address = creatorAccount.Address,
                KeyId = creatorAccountKey.Index,
                SequenceNumber = creatorAccountKey.SequenceNumber
            };

            // get the latest sealed block to use as a reference block
            var latestBlock = await _flowClient.GetLatestBlockAsync();

            tx.ReferenceBlockId = latestBlock.Id;

            // sign and submit the transaction
            tx.AddEnvelopeSignature(creatorAccount.Address, creatorAccountKey.Index, creatorAccountKey.Signer);
            await _flowClient.SendTransactionAsync(tx);
        }
    }
}
