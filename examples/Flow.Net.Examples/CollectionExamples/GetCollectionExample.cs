using Flow.Net.Sdk;
using Flow.Net.Sdk.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Flow.Net.Examples.CollectionExamples
{
    public class GetCollectionExample : GrpcExampleBase
    {
        public static async Task RunAsync()
        {
            Console.WriteLine("\nRunning GetCollectionExample\n");
            await CreateFlowClientAsync();
            var collectionGuarantee = await PrepCollectionId();
            await Demo(collectionGuarantee);
            Console.WriteLine("\nGetCollectionExample Complete\n");
        }

        private static async Task Demo(FlowCollectionGuarantee flowCollectionGuarantee)
        {
            // get collection by ID
            var collection = await FlowClient.GetCollectionByIdAsync(flowCollectionGuarantee.CollectionId);
            PrintCollection(collection);
        }

        private static void PrintCollection(FlowCollectionResponse flowCollection)
        {
            Console.WriteLine($"ID: {flowCollection.Id.FromByteStringToHex()}");
            Console.WriteLine("Transactions: [{0}]", string.Join(", ", flowCollection.TransactionIds.Select(s => s.FromByteStringToHex()).ToArray()));
        }

        private static async Task<FlowCollectionGuarantee> PrepCollectionId()
        {
            // create account
            var flowAccountKey = FlowAccountKey.GenerateRandomEcdsaKey(SignatureAlgo.ECDSA_P256, HashAlgo.SHA3_256);
            await CreateAccountAsync(new List<FlowAccountKey> { flowAccountKey });

            var block = await FlowClient.GetBlockByHeightAsync(1);
            return block.CollectionGuarantees.FirstOrDefault();
        }
    }
}
