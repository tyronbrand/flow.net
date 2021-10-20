using Flow.Net.Sdk;
using Flow.Net.Sdk.Client;
using Flow.Net.Sdk.Extensions;
using Flow.Net.Sdk.Models;
using Flow.Net.Sdk.Templates;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Flow.Net.Examples
{
    public class CreateAccountExample
    {
        public static readonly string networkUrl = "127.0.0.1:3569"; // emulator

        public static async Task<FlowSendTransactionResponse> RunAsync()
        {
            // create a flow client
            var _flowClient = FlowClientAsync.Create(networkUrl);

            // creator (typically a service account)
            var creatorAccount = await _flowClient.ReadAccountFromConfigAsync("emulator-account");

            // generate our new account key
            var flowAccountKey = FlowAccountKey.NewEcdsaAccountKey(SignatureAlgo.ECDSA_secp256k1, HashAlgo.SHA3_256, 1000);

            // use template to create a transaction
            var tx = Account.CreateAccount(new List<FlowAccountKey> { flowAccountKey }, creatorAccount.Address);                       

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
            return await _flowClient.SendTransactionAsync(tx);
        }
    }
}
