using Flow.Net.Examples.Utilities;
using Flow.Net.Sdk;
using Flow.Net.Sdk.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Flow.Net.Examples
{
    public class SinglePartyMultiSignatureExample : ExampleBase
    {
        public static async Task RunAsync()
        {
            await CreateFlowClientAsync();

            ColorConsole.WriteWrappedHeader("Single party multi signature example", headerColor: ConsoleColor.Yellow, dashColor: ConsoleColor.Yellow);

            var flowAccountKey1 = FlowAccountKey.NewEcdsaAccountKey(SignatureAlgo.ECDSA_P256, HashAlgo.SHA3_256, 500);
            var flowAccountKey2 = FlowAccountKey.NewEcdsaAccountKey(SignatureAlgo.ECDSA_P256, HashAlgo.SHA3_256, 500);

            ColorConsole.WriteWrappedSubHeader("Creating new account");
            var account1 = await CreateAccountAsync(new List<FlowAccountKey> { flowAccountKey1, flowAccountKey2 });
            var account1Key1 = account1.Keys[0];
            var account1Key2 = account1.Keys[1];            

            ColorConsole.WriteWrappedSubHeader("Creating our transaction");

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
            ConvertToConsoleMessage.WriteInfoMessage(tx);
            var txResponse = await _flowClient.SendTransactionAsync(tx);

            // wait for seal
            var sealedResponse = await _flowClient.WaitForSealAsync(txResponse);

            if (sealedResponse.Status == Sdk.Protos.entities.TransactionStatus.Sealed)
                ConvertToConsoleMessage.WriteSuccessMessage(sealedResponse);

            ColorConsole.WriteWrappedHeader("End single party multi signature example", headerColor: ConsoleColor.Yellow, dashColor: ConsoleColor.Gray);
        }
    }
}
