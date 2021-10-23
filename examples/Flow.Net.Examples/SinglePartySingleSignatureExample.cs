using Flow.Net.Examples.Utilities;
using Flow.Net.Sdk;
using Flow.Net.Sdk.Models;
using System;
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

            ColorConsole.WriteWrappedHeader("Single party single signature example", headerColor: ConsoleColor.Yellow, dashColor: ConsoleColor.Yellow);

            var flowAccountKey = FlowAccountKey.NewEcdsaAccountKey(SignatureAlgo.ECDSA_P256, HashAlgo.SHA3_256, 1000);

            ColorConsole.WriteWrappedSubHeader("Creating new account");
            var account1 = await CreateAccountAsync(new List<FlowAccountKey> { flowAccountKey });
            var account1Key = account1.Keys.FirstOrDefault();

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
            ConvertToConsoleMessage.WriteInfoMessage(tx);
            var txResponse = await _flowClient.SendTransactionAsync(tx);

            // wait for seal
            var sealedResponse = await _flowClient.WaitForSealAsync(txResponse);

            if (sealedResponse.Status == Sdk.Protos.entities.TransactionStatus.Sealed)
                ConvertToConsoleMessage.WriteSuccessMessage(sealedResponse);

            ColorConsole.WriteWrappedHeader("End Single party single signature example", headerColor: ConsoleColor.Yellow, dashColor: ConsoleColor.Gray);
        }
    }
}
