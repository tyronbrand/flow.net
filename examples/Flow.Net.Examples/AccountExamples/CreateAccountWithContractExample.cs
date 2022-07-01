using Flow.Net.Sdk.Client.Http.Templates;
using Flow.Net.Sdk.Core;
using Flow.Net.Sdk.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Flow.Net.Examples.AccountExamples
{
    public class CreateAccountWithContractExample : ExampleBase
    {
        public static async Task RunAsync()
        {
            Console.WriteLine("\nRunning CreateAccountWithContractExample\n");
            await CreateFlowClientAsync();
            await Demo();
            Console.WriteLine("\nCreateAccountWithContractExample Complete\n");
        }

        private static async Task Demo()
        {
            // read flow.json
            var config = Utilities.ReadConfig();
            // get account from config
            var accountConfig = config.Accounts["emulator-account"];
            // get service account at latest block
            var creatorAccount = await FlowClient.GetAccountAtLatestBlockAsync(accountConfig.Address);
            // add a Signer with the serviceAccount and the accountConfig
            creatorAccount = Utilities.AddSignerFromConfigAccount(accountConfig, creatorAccount);

            // generate our new account key
            var flowAccountKey = FlowAccountKey.GenerateRandomEcdsaKey(SignatureAlgo.ECDSA_secp256k1, HashAlgo.SHA3_256);

            // contract to deploy
            var helloWorldContract = Utilities.ReadCadenceScript("hello-world-contract");
            var flowContract = new FlowContract
            {
                Name = "HelloWorld",
                Source = helloWorldContract
            };

            // use template to create a transaction
            var tx = AccountTemplates.CreateAccount(new List<FlowAccountKey> { flowAccountKey }, creatorAccount.Address, new List<FlowContract> { flowContract });

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
            var latestBlock = await FlowClient.GetLatestBlockAsync();
            tx.ReferenceBlockId = latestBlock.Header.Id;

            // sign and submit the transaction
            tx = FlowTransaction.AddEnvelopeSignature(tx, creatorAccount.Address, creatorAccountKey.Index, creatorAccountKey.Signer);
            
            var response = await FlowClient.SendTransactionAsync(tx);

            // wait for seal
            var sealedResponse = await FlowClient.WaitForSealAsync(response.Id);

            if (sealedResponse.Status == TransactionStatus.Sealed)
                PrintResult(flowContract);
        }

        private static void PrintResult(FlowContract flowContract)
        {
            Console.WriteLine("Account created with contract:");
            Console.WriteLine($"Name: {flowContract.Name}");
            Console.WriteLine($"Source: {flowContract.Source}");
        }
    }
}
