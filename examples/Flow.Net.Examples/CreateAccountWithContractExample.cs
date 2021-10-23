using Flow.Net.Examples.Utilities;
using Flow.Net.Sdk;
using Flow.Net.Sdk.Models;
using Flow.Net.Sdk.Templates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Flow.Net.Examples
{
    public class CreateAccountWithContractExample : ExampleBase
    {
        public static async Task RunAsync()
        {
            await CreateFlowClientAsync();

            ColorConsole.WriteWrappedHeader("Create account with contract example", headerColor: ConsoleColor.Yellow, dashColor: ConsoleColor.Yellow);

            // creator (typically a service account)
            var creatorAccount = await _flowClient.ReadAccountFromConfigAsync("emulator-account");

            // generate our new account key
            var flowAccountKey = FlowAccountKey.NewEcdsaAccountKey(SignatureAlgo.ECDSA_secp256k1, HashAlgo.SHA3_256, 1000);
            ConvertToConsoleMessage.WriteInfoMessage(flowAccountKey);

            // contract to deploy
            var helloWorldContract = Sdk.Utilities.ReadCadenceScript("hello-world-contract");
            var flowContract = new FlowContract
            {
                Name = "HelloWorld",
                Source = helloWorldContract
            };
            ConvertToConsoleMessage.WriteInfoMessage(flowContract);

            // use template to create a transaction
            ColorConsole.WriteWrappedSubHeader("Creating new account");
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
            
            ConvertToConsoleMessage.WriteInfoMessage(tx);

            var response = await _flowClient.SendTransactionAsync(tx);

            // wait for seal
            var sealedResponse = await _flowClient.WaitForSealAsync(response);

            if (sealedResponse.Status == Sdk.Protos.entities.TransactionStatus.Sealed)
                ConvertToConsoleMessage.WriteSuccessMessage(sealedResponse);

            ColorConsole.WriteWrappedHeader("End account with contract example", headerColor: ConsoleColor.Yellow, dashColor: ConsoleColor.Gray);
        }
    }
}
