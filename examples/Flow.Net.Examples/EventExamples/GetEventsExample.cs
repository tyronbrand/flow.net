using Flow.Net.Sdk.Core.Templates;
using Flow.Net.Sdk.Core;
using Flow.Net.Sdk.Core.Cadence;
using Flow.Net.Sdk.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Flow.Net.Sdk.Core.Client;

namespace Flow.Net.Examples.EventExamples
{
    public class GetEventsExample : ExampleBase
    {
        public static async Task RunAsync(IFlowClient flowClient)
        {
            Console.WriteLine("\nRunning GetEventsExample\n");
            FlowClient = flowClient;
            var flowAccount = await PrepFlowAccountWithContract();
            if(flowAccount != null)
            {
                var flowTransactionId = await PrepFlowTransaction(flowAccount);
                await Demo(flowAccount, flowTransactionId);
            }
            Console.WriteLine("\nGetEventsExample Complete\n");
        }

        private static async Task Demo(FlowAccount flowAccount, string flowTransactionId)
        {
            // Query for account creation events by type
            var eventsForHeightRange = await FlowClient.GetEventsForHeightRangeAsync("flow.AccountCreated", 0, 100);
            Console.Write("\n---------- Query for account creation events by type ----------\n");
            PrintEvents(eventsForHeightRange);

            // Query for our custom event by type
            var customType = $"A.{flowAccount.Address.Address}.EventDemo.Add";
            var customEventsForHeightRange = await FlowClient.GetEventsForHeightRangeAsync(customType, 0, 100);
            Console.Write($"\n---------- Query for our custom event by type ({customType}) ----------\n");
            PrintEvents(customEventsForHeightRange);

            // Get events directly from transaction result
            var txResult = await FlowClient.GetTransactionResultAsync(flowTransactionId);
            Console.Write("\n---------- Get events directly from transaction result ----------\n");
            PrintEvent(txResult.Events);
        }

        private static void PrintEvents(IEnumerable<FlowBlockEvent> flowBlockEvents)
        {
            foreach (var block in flowBlockEvents)
            {
                Console.WriteLine("\n------------------------------------------------------------------------------------------------------------");
                if (block.Events.Any())
                {
                    Console.WriteLine($"{block.Events.Count()} event(s) at block height: {block.BlockHeight}\n");
                    Console.WriteLine($"Block Id: {block.BlockId}\n");
                    Console.WriteLine($"Block Timestamp: {block.BlockTimestamp}\n");
                    PrintEvent(block.Events);
                }
                else
                {
                    Console.WriteLine($"No events at block height: {block.BlockHeight}");
                }
                Console.WriteLine("------------------------------------------------------------------------------------------------------------\n");
            }
        }

        private static void PrintEvent(IEnumerable<FlowEvent> flowEvents)
        {
            foreach(var @event in flowEvents)
            {
                Console.WriteLine($"Type: {@event.Type}");
                Console.WriteLine($"Values: {@event.Payload.Encode()}");
                Console.WriteLine($"Transaction ID: {@event.TransactionId} \n");
            }
        }

        private static async Task<FlowAccount> PrepFlowAccountWithContract()
        {
            // read flow.json
            var config = Utilities.ReadConfig();
            // get account from config
            var accountConfig = config.Accounts["emulator-account"];
            // get service account at latest block
            var serviceAccount = await FlowClient.GetAccountAtLatestBlockAsync(accountConfig.Address);
            // we can create a Signer with the serviceAccount and the accountConfig
            serviceAccount = Utilities.AddSignerFromConfigAccount(accountConfig, serviceAccount);
            // creator key to use
            var creatorAccountKey = serviceAccount.Keys.FirstOrDefault();

            // generate our new account key
            var flowAccountKey = FlowAccountKey.GenerateRandomEcdsaKey(SignatureAlgo.ECDSA_secp256k1, HashAlgo.SHA3_256);

            // contract to deploy
            var contract = @"
pub contract EventDemo {
    pub event Add(x: Int, y: Int, sum: Int)
	pub fun add(x: Int, y: Int)
    {
        let sum = x + y

        emit Add(x: x, y: y, sum: sum)
	}
}";
            var flowContract = new FlowContract
            {
                Name = "EventDemo",
                Source = contract
            };

            // use template to create a transaction
            var tx = AccountTemplates.CreateAccount(
                new List<FlowAccountKey> 
                {
                    flowAccountKey
                },
                serviceAccount.Address,
                new List<FlowContract>
                {
                    flowContract
                });

            // set the transaction payer and proposal key
            tx.Payer = serviceAccount.Address;
            tx.ProposalKey = new FlowProposalKey
            {
                Address = serviceAccount.Address,
                KeyId = creatorAccountKey.Index,
                SequenceNumber = creatorAccountKey.SequenceNumber
            };

            // get the latest sealed block to use as a reference block
            var latestBlock = await FlowClient.GetLatestBlockAsync();
            tx.ReferenceBlockId = latestBlock.Header.Id;

            // sign
            tx = FlowTransaction.AddEnvelopeSignature(tx, serviceAccount.Address, creatorAccountKey.Index, creatorAccountKey.Signer);

            // send transaction
            var response = await FlowClient.SendTransactionAsync(tx);

            // wait for seal
            var sealedResponse = await FlowClient.WaitForSealAsync(response.Id);

            if (sealedResponse.Status != TransactionStatus.Sealed)
                return null;
            
            // get newly created accounts address
            var newAccountAddress = sealedResponse.Events.AccountCreatedAddress();

            // get new account details
            var newAccount = await FlowClient.GetAccountAtLatestBlockAsync(newAccountAddress.Address);
            newAccount.Keys = FlowAccountKey.UpdateFlowAccountKeys(new List<FlowAccountKey> { flowAccountKey }, newAccount.Keys);
            return newAccount;
        }

        private static async Task<string> PrepFlowTransaction(FlowAccount flowAccount)
        {
            // key to use
            var flowAccountKey = flowAccount.Keys.FirstOrDefault();

            // Send a tx that emits the event in the deployed contract
            var script = @$"
import EventDemo from 0x{flowAccount.Address.Address}
transaction {{
	execute {{
		EventDemo.add(x: 2, y: 3)
	}}
}}";

            // get the latest sealed block to use as a reference block
            var latestBlock = await FlowClient.GetLatestBlockAsync();

            // create transaction
            var tx = new FlowTransaction
            {
                Script = script,
                Payer = flowAccount.Address,
                ProposalKey = new FlowProposalKey
                {
                    Address = flowAccount.Address,
                    KeyId = flowAccountKey.Index,
                    SequenceNumber = flowAccountKey.SequenceNumber
                },
                ReferenceBlockId = latestBlock.Header.Id
            };

            // sign
            tx = FlowTransaction.AddEnvelopeSignature(tx, flowAccount.Address, flowAccountKey.Index, flowAccountKey.Signer);

            // send transaction
            var response = await FlowClient.SendTransactionAsync(tx);

            // wait for seal
            await FlowClient.WaitForSealAsync(response.Id);

            return response.Id;
        }
    }
}
