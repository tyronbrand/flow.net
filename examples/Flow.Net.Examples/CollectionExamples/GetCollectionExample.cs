using Flow.Net.Sdk;
using Flow.Net.Sdk.Models;
using Google.Protobuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flow.Net.Examples
{
    public class GetCollectionExample : ExampleBase
    {
        public static async Task RunAsync()
        {
            await CreateFlowClientAsync();
            var collectionGuarantee = await PrepCollectionId();
            await Demo(collectionGuarantee);
        }

        private static async Task Demo(FlowCollectionGuarantee flowCollectionGuarantee)
        {
            // get collection by ID
            var collection = await _flowClient.GetCollectionByIdAsync(flowCollectionGuarantee.CollectionId);
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
            var flowAccountKey = FlowAccountKey.NewEcdsaAccountKey(SignatureAlgo.ECDSA_P256, HashAlgo.SHA3_256, 1000);
            await CreateAccountAsync(new List<FlowAccountKey> { flowAccountKey });

            var block = await _flowClient.GetBlockByHeightAsync(1);
            return block.CollectionGuarantees.FirstOrDefault();
        }
    }
}
