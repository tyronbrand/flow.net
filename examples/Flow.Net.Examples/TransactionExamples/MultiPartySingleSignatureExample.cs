using Flow.Net.Sdk;
using Flow.Net.Sdk.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Flow.Net.Examples.TransactionExamples
{
    public class MultiPartySingleSignatureExample : ExampleBase
    {
        public static async Task RunAsync()
        {
            Console.WriteLine("\nRunning MultiPartySingleSignatureExample\n");
            await CreateFlowClientAsync();
            await Demo();
            Console.WriteLine("\nMultiPartySingleSignatureExample Complete\n");
        }

        private static async Task Demo()
        {
            // generate key for account1
            var flowAccountKey1 = FlowAccountKey.GenerateRandomEcdsaKey(SignatureAlgo.ECDSA_P256, HashAlgo.SHA3_256);
            // create account1
            var account1 = await CreateAccountAsync(new List<FlowAccountKey> { flowAccountKey1 });
            // select account1 key
            var account1Key = account1.Keys.FirstOrDefault();

            // generate key for account2
            var flowAccountKey2 = FlowAccountKey.GenerateRandomEcdsaKey(SignatureAlgo.ECDSA_P256, HashAlgo.SHA3_256);
            // create account2
            var account2 = await CreateAccountAsync(new List<FlowAccountKey> { flowAccountKey2 });
            // select account2 key
            var account2Key = account2.Keys.FirstOrDefault();

            // get the latest sealed block to use as a reference block
            var latestBlock = await FlowClient.GetLatestBlockAsync();

            var tx = new FlowTransaction
            {
                Script = new FlowCadenceScript
                {
                    Script = "transaction {prepare(signer: AuthAccount) { log(signer.address) }}"
                },
                GasLimit = 9999,
                Payer = account2.Address,
                ProposalKey = new FlowProposalKey
                {
                    Address = account1.Address,
                    KeyId = account1Key.Index,
                    SequenceNumber = account1Key.SequenceNumber
                },
                ReferenceBlockId = latestBlock.Id
            };

            // authorizers
            tx.Authorizers.Add(account1.Address);

            // account 1 signs the payload with key 1
            tx = FlowTransaction.AddPayloadSignature(tx, account1.Address, account1Key.Index, account1Key.Signer);

            // account 2 signs the envelope
            tx = FlowTransaction.AddEnvelopeSignature(tx, account2.Address, account2Key.Index, account2Key.Signer);

            // send transaction
            var txResponse = await FlowClient.SendTransactionAsync(tx);

            // wait for seal
            await FlowClient.WaitForSealAsync(txResponse);
        }
    }
}
