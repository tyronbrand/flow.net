using Flow.Net.Examples.Utilities;
using Flow.Net.Sdk;
using Flow.Net.Sdk.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Flow.Net.Examples
{
    public class MultiPartyMultiSignatureExample : ExampleBase
    {
        public static async Task RunAsync()
        {
            await CreateFlowClientAsync();

            ColorConsole.WriteWrappedHeader("Multi party multi signature example", headerColor: ConsoleColor.Yellow, dashColor: ConsoleColor.Yellow);

            var flowAccount1Key1 = FlowAccountKey.NewEcdsaAccountKey(SignatureAlgo.ECDSA_P256, HashAlgo.SHA2_256, 500);
            var flowAccount1Key2 = FlowAccountKey.NewEcdsaAccountKey(SignatureAlgo.ECDSA_P256, HashAlgo.SHA2_256, 500);
            ColorConsole.WriteWrappedSubHeader("Creating new account 1");
            var account1 = await CreateAccountAsync(new List<FlowAccountKey> { flowAccount1Key1, flowAccount1Key2 });

            var flowAccount2Key3 = FlowAccountKey.NewEcdsaAccountKey(SignatureAlgo.ECDSA_P256, HashAlgo.SHA3_256, 500);
            var flowAccount2Key4 = FlowAccountKey.NewEcdsaAccountKey(SignatureAlgo.ECDSA_P256, HashAlgo.SHA3_256, 500);
            ColorConsole.WriteWrappedSubHeader("Creating new account 2");
            var account2 = await CreateAccountAsync(new List<FlowAccountKey> { flowAccount2Key3, flowAccount2Key4 });

            ColorConsole.WriteWrappedSubHeader("Creating our transaction");
            var lastestBlock = await _flowClient.GetLatestBlockAsync();
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
                ReferenceBlockId = lastestBlock.Id
            };

            // authorizers
            tx.Authorizers.Add(account1.Address);

            // account 1 signs the payload with key 1
            tx.AddPayloadSignature(account1.Address, account1.Keys[0].Index, account1.Keys[0].Signer);

            // account 1 signs the payload with key 2
            tx.AddPayloadSignature(account1.Address, account1.Keys[1].Index, account1.Keys[1].Signer);

            // account 2 signs the envelope with key 3
            tx.AddEnvelopeSignature(account2.Address, account2.Keys[0].Index, account2.Keys[0].Signer);

            // account 2 signs the envelope with key 3
            tx.AddEnvelopeSignature(account2.Address, account2.Keys[1].Index, account2.Keys[1].Signer);

            // send transaction
            ConvertToConsoleMessage.WriteInfoMessage(tx);
            var txResponse = await _flowClient.SendTransactionAsync(tx);

            // wait for seal
            var sealedResponse = await _flowClient.WaitForSealAsync(txResponse);

            if (sealedResponse.Status == Sdk.Protos.entities.TransactionStatus.Sealed)
                ConvertToConsoleMessage.WriteSuccessMessage(sealedResponse);

            ColorConsole.WriteWrappedHeader("End multi party multi signature example", headerColor: ConsoleColor.Yellow, dashColor: ConsoleColor.Gray);
        }
    }
}
