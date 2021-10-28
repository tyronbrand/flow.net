using Flow.Net.Sdk;
using Flow.Net.Sdk.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Flow.Net.Examples
{
    public class SinglePartySingleSignatureExample : ExampleBase
    {       
        public static async Task RunAsync()
        {
            await CreateFlowClientAsync();
            await Demo();
        }

        private static async Task Demo()
        {
            // generate key
            var flowAccountKey = FlowAccountKey.NewEcdsaAccountKey(SignatureAlgo.ECDSA_P256, HashAlgo.SHA3_256, 1000);           
            // create account with key
            var account1 = await CreateAccountAsync(new List<FlowAccountKey> { flowAccountKey });
            // select key
            var account1Key = account1.Keys.FirstOrDefault();

            // get the latest sealed block to use as a reference block
            var lastestBlock = await _flowClient.GetLatestBlockAsync();
            
            var tx = new FlowTransaction
            {
                Script = "transaction {prepare(signer: AuthAccount) { log(signer.address) }}",
                GasLimit = 100,
                Payer = account1.Address,
                ProposalKey = new FlowProposalKey
                {
                    Address = account1.Address,
                    KeyId = account1Key.Index,
                    SequenceNumber = account1Key.SequenceNumber
                },
                ReferenceBlockId = lastestBlock.Id
            };

            // authorizers
            tx.Authorizers.Add(account1.Address);

            // account 1 signs the envelope with key 1
            tx.AddEnvelopeSignature(account1.Address, account1Key.Index, account1Key.Signer);

            // send transaction
            var txResponse = await _flowClient.SendTransactionAsync(tx);

            // wait for seal
            await _flowClient.WaitForSealAsync(txResponse);
        }
    }
}
