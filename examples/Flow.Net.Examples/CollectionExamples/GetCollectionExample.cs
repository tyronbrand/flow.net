using Flow.Net.Sdk.Core;
using Flow.Net.Sdk.Core.Client;
using Flow.Net.Sdk.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Flow.Net.Examples.CollectionExamples
{
    public class GetCollectionExample : ExampleBase
    {
        public static async Task RunAsync(IFlowClient flowClient)
        {
            Console.WriteLine("\nRunning GetCollectionExample\n");
            FlowClient = flowClient;
            var collectionGuarantee = await PrepCollectionId();
            await Demo(collectionGuarantee);
            Console.WriteLine("\nGetCollectionExample Complete\n");
        }

        private static async Task Demo(FlowCollectionGuarantee flowCollectionGuarantee)
        {
            // get collection by ID
            var collection = await FlowClient.GetCollectionAsync(flowCollectionGuarantee.CollectionId);
            PrintCollection(collection);
        }

        private static void PrintCollection(FlowCollection flowCollection)
        {
            Console.WriteLine($"ID: {flowCollection.Id}");
            Console.WriteLine("Transactions: [{0}]", string.Join(", ", flowCollection.TransactionIds.Select(s => s.Id).ToArray()));
        }

        private static async Task<FlowCollectionGuarantee> PrepCollectionId()
        {
            // create account
            var flowAccountKey = FlowAccountKey.GenerateRandomEcdsaKey(SignatureAlgo.ECDSA_P256, HashAlgo.SHA3_256);
            await CreateAccountAsync(new List<FlowAccountKey> { flowAccountKey });

            var block = await FlowClient.GetBlockByHeightAsync(1);
            return block.Payload.CollectionGuarantees.FirstOrDefault();
        }
    }
}
