using Flow.Net.Sdk;
using Flow.Net.Sdk.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Flow.Net.Examples
{
    public class MultiPartyTwoAuthorizersExample : ExampleBase
    {
        public static async Task RunAsync()
        {
            await CreateFlowClientAsync();
            await Demo();
        }

        private static async Task Demo()
        {
            // generate key for account1
            var flowAccountKey1 = FlowAccountKey.NewEcdsaAccountKey(SignatureAlgo.ECDSA_P256, HashAlgo.SHA3_256, 1000);
            // create account1
            var account1 = await CreateAccountAsync(new List<FlowAccountKey> { flowAccountKey1 });
            // select account1 key
            var account1Key = account1.Keys.FirstOrDefault();

            // generate key for account2
            var flowAccountKey2 = FlowAccountKey.NewEcdsaAccountKey(SignatureAlgo.ECDSA_P256, HashAlgo.SHA3_256, 1000);
            // create account2
            var account2 = await CreateAccountAsync(new List<FlowAccountKey> { flowAccountKey2 });
            // select account2 key
            var account2Key = account2.Keys.FirstOrDefault();

            // get the latest sealed block to use as a reference block
            var lastestBlock = await _flowClient.GetLatestBlockAsync();

            var tx = new FlowTransaction
            {
                Script = @"
transaction { 
	prepare(signer1: AuthAccount, signer2: AuthAccount) { 
		log(signer1.address) 
		log(signer2.address)
	}
}",
                GasLimit = 9999,
                Payer = account2.Address,
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
            tx.Authorizers.Add(account2.Address);

            // account 1 signs the payload with key 1
            tx.AddPayloadSignature(account1.Address, account1Key.Index, account1Key.Signer);

            // account 2 signs the envelope
            tx.AddEnvelopeSignature(account2.Address, account2Key.Index, account2Key.Signer);

            // send transaction
            var txResponse = await _flowClient.SendTransactionAsync(tx);

            // wait for seal
            await _flowClient.WaitForSealAsync(txResponse);
        }
    }
}
