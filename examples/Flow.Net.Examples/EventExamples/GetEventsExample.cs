using Flow.Net.Sdk;
using Flow.Net.Sdk.Cadence;
using Flow.Net.Sdk.Models;
using Flow.Net.Sdk.Templates;
using Google.Protobuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Flow.Net.Examples.EventExamples
{
    public class GetEventsExample : ExampleBase
    {
        public static async Task RunAsync()
        {
            Console.WriteLine("\nRunning GetEventsExample\n");
            await CreateFlowClientAsync();
            var flowAccount = await PrepFlowAccountWithContract();
            if(flowAccount != null)
            {
                var flowTransactionId = await PrepFlowTransaction(flowAccount);
                await Demo(flowAccount, flowTransactionId);
            }
            Console.WriteLine("\nGetEventsExample Complete\n");
        }

        private static async Task Demo(FlowAccount flowAccount, ByteString flowTransactionId)
        {
            // Query for account creation events by type
            var eventsForHeightRange = await FlowClient.GetEventsForHeightRangeAsync("flow.AccountCreated", 0, 100);
            PrintEvents(eventsForHeightRange);

            // Query for our custom event by type
            var customType = $"A.{flowAccount.Address.HexValue}.EventDemo.Add";
            var customEventsForHeightRange = await FlowClient.GetEventsForHeightRangeAsync(customType, 0, 100);
            PrintEvents(customEventsForHeightRange);

            // Get events directly from transaction result
            var txResult = await FlowClient.GetTransactionResultAsync(flowTransactionId);
            PrintEvent(txResult.Events);
        }

        private static void PrintEvents(IEnumerable<FlowBlockEvent> flowBlockEvents)
        {
            foreach(var blockEvent in flowBlockEvents)
                PrintEvent(blockEvent.Events);
        }

        private static void PrintEvent(IEnumerable<FlowEvent> flowEvents)
        {
            foreach(var @event in flowEvents)
            {
                Console.WriteLine($"Type: {@event.Type}");
                Console.WriteLine($"Values: {@event.Payload.Encode()}");
                Console.WriteLine($"Transaction ID: {@event.TransactionId.FromByteStringToHex()} \n");
            }
        }

        private static async Task<FlowAccount> PrepFlowAccountWithContract()
        {
            // creator (typically a service account)
            var creatorAccount = await FlowClient.ReadAccountFromConfigAsync("emulator-account");
            // creator key to use
            var creatorAccountKey = creatorAccount.Keys.FirstOrDefault();

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
            var tx = Account.CreateAccount(
                new List<FlowAccountKey> 
                {
                    flowAccountKey
                },
                creatorAccount.Address,
                new List<FlowContract>
                {
                    flowContract
                });

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
            tx.ReferenceBlockId = latestBlock.Id;

            // sign
            tx = FlowTransaction.AddEnvelopeSignature(tx, creatorAccount.Address, creatorAccountKey.Index, creatorAccountKey.Signer);

            // send transaction
            var response = await FlowClient.SendTransactionAsync(tx);

            // wait for seal
            var sealedResponse = await FlowClient.WaitForSealAsync(response);

            if (sealedResponse.Status != Sdk.Protos.entities.TransactionStatus.Sealed)
                return null;
            
            // get newly created accounts address
            var newAccountAddress = sealedResponse.Events.AccountCreatedAddress();

            // get new account details
            var newAccount = await FlowClient.GetAccountAtLatestBlockAsync(newAccountAddress);
            newAccount.Keys = FlowAccountKey.UpdateFlowAccountKeys(new List<FlowAccountKey> { flowAccountKey }, newAccount.Keys);
            return newAccount;
        }

        private static async Task<ByteString> PrepFlowTransaction(FlowAccount flowAccount)
        {
            // key to use
            var flowAccountKey = flowAccount.Keys.FirstOrDefault();

            // Send a tx that emits the event in the deployed contract
            var script = @$"
import EventDemo from 0x{flowAccount.Address.HexValue}
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
                Script = new FlowCadenceScript
                {
                    Script = script
                },
                Payer = flowAccount.Address,
                ProposalKey = new FlowProposalKey
                {
                    Address = flowAccount.Address,
                    KeyId = flowAccountKey.Index,
                    SequenceNumber = flowAccountKey.SequenceNumber
                },
                ReferenceBlockId = latestBlock.Id
            };

            // sign
            tx = FlowTransaction.AddEnvelopeSignature(tx, flowAccount.Address, flowAccountKey.Index, flowAccountKey.Signer);

            // send transaction
            var response = await FlowClient.SendTransactionAsync(tx);

            // wait for seal
            await FlowClient.WaitForSealAsync(response);

            return response.Id;
        }
    }
}
