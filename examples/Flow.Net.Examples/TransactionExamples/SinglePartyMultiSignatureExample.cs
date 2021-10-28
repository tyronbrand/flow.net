﻿using Flow.Net.Sdk;
using Flow.Net.Sdk.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Flow.Net.Examples
{
    public class SinglePartyMultiSignatureExample : ExampleBase
    {
        public static async Task RunAsync()
        {
            await CreateFlowClientAsync();
            await Demo();
        }

        private static async Task Demo()
        {
            // generate key 1 for account1
            var flowAccountKey1 = FlowAccountKey.NewEcdsaAccountKey(SignatureAlgo.ECDSA_P256, HashAlgo.SHA3_256, 500);
            // generate key 2 for account1
            var flowAccountKey2 = FlowAccountKey.NewEcdsaAccountKey(SignatureAlgo.ECDSA_P256, HashAlgo.SHA3_256, 500);

            // create account with keys
            var account1 = await CreateAccountAsync(new List<FlowAccountKey> { flowAccountKey1, flowAccountKey2 });

            // select keys
            var account1Key1 = account1.Keys[0];
            var account1Key2 = account1.Keys[1];

            // get the latest sealed block to use as a reference block
            var lastestBlock = await _flowClient.GetLatestBlockAsync();

            var tx = new FlowTransaction
            {
                Script = "transaction {prepare(signer: AuthAccount) { log(signer.address) }}",
                GasLimit = 9999,
                Payer = account1.Address,
                ProposalKey = new FlowProposalKey
                {
                    Address = account1.Address,
                    KeyId = account1Key1.Index,
                    SequenceNumber = account1Key1.SequenceNumber
                },
                ReferenceBlockId = lastestBlock.Id
            };

            // authorizers
            tx.Authorizers.Add(account1.Address);

            // account 1 signs the envelope with key 1
            tx.AddEnvelopeSignature(account1.Address, account1Key1.Index, account1Key1.Signer);

            // account 1 signs the envelope with key 2
            tx.AddEnvelopeSignature(account1.Address, account1Key2.Index, account1Key2.Signer);

            // send transaction
            var txResponse = await _flowClient.SendTransactionAsync(tx);

            // wait for seal
            await _flowClient.WaitForSealAsync(txResponse);
        }
    }
}