using Flow.Net.Sdk;
using Flow.Net.Sdk.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Flow.Net.Examples.TransactionExamples
{
    public class MultiPartyMultiSignatureExample : GrpcExampleBase
    {
        public static async Task RunAsync()
        {
            Console.WriteLine("\nRunning MultiPartyMultiSignatureExample\n");
            await CreateFlowClientAsync();
            await Demo();
            Console.WriteLine("\nMultiPartyMultiSignatureExample Complete\n");
        }

        private static async Task Demo()
        {
            // generate key 1 for account1
            var flowAccount1Key1 = FlowAccountKey.GenerateRandomEcdsaKey(SignatureAlgo.ECDSA_P256, HashAlgo.SHA2_256, 500);
            // generate key 2 for account1
            var flowAccount1Key2 = FlowAccountKey.GenerateRandomEcdsaKey(SignatureAlgo.ECDSA_P256, HashAlgo.SHA2_256, 500);
            // create account1
            var account1 = await CreateAccountAsync(new List<FlowAccountKey> { flowAccount1Key1, flowAccount1Key2 });

            // generate key 1 for account2
            var flowAccount2Key3 = FlowAccountKey.GenerateRandomEcdsaKey(SignatureAlgo.ECDSA_P256, HashAlgo.SHA3_256, 500);
            // generate key 2 for account2
            var flowAccount2Key4 = FlowAccountKey.GenerateRandomEcdsaKey(SignatureAlgo.ECDSA_P256, HashAlgo.SHA3_256, 500);
            // create account2
            var account2 = await CreateAccountAsync(new List<FlowAccountKey> { flowAccount2Key3, flowAccount2Key4 });

            // get the latest sealed block to use as a reference block
            var latestBlock = await FlowClient.GetLatestBlockAsync();

            var tx = new FlowTransaction
            {
                Script = "transaction {prepare(signer: AuthAccount) { log(signer.address) }}",
                GasLimit = 9999,
                Payer = account2.Address,
                ProposalKey = new FlowProposalKey
                {
                    Address = account1.Address,
                    KeyId = account1.Keys[0].Index,
                    SequenceNumber = account1.Keys[0].SequenceNumber
                },
                ReferenceBlockId = latestBlock.Id
            };

            // authorizers
            tx.Authorizers.Add(account1.Address);

            // account 1 signs the payload with key 1
            tx = FlowTransaction.AddPayloadSignature(tx, account1.Address, account1.Keys[0].Index, account1.Keys[0].Signer);

            // account 1 signs the payload with key 2
            tx = FlowTransaction.AddPayloadSignature(tx, account1.Address, account1.Keys[1].Index, account1.Keys[1].Signer);

            // account 2 signs the envelope with key 3
            tx = FlowTransaction.AddEnvelopeSignature(tx, account2.Address, account2.Keys[0].Index, account2.Keys[0].Signer);

            // account 2 signs the envelope with key 3
            tx = FlowTransaction.AddEnvelopeSignature(tx, account2.Address, account2.Keys[1].Index, account2.Keys[1].Signer);

            // send transaction
            var txResponse = await FlowClient.SendTransactionAsync(tx);

            // wait for seal
            await FlowClient.WaitForSealAsync(txResponse);
        }
    }
}
