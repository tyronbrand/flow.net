using Flow.Net.Sdk.Core;
using Flow.Net.Sdk.Core.Client;
using Flow.Net.Sdk.Core.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Flow.Net.Examples.AccountExamples
{
    public class CreateAccountExample : ExampleBase
    {
        public static async Task RunAsync(IFlowClient flowClient)
        {
            Console.WriteLine("\nRunning CreateAccountExample\n");
            FlowClient = flowClient;

            // generate our new account key
            var flowAccountKey = FlowAccountKey.GenerateRandomEcdsaKey(SignatureAlgo.ECDSA_P256, HashAlgo.SHA3_256);

            // example found in base class
            var account = await CreateAccountAsync(new List<FlowAccountKey> { flowAccountKey });
            PrintResult(account);

            Console.WriteLine("\nCreateAccountExample Complete\n");
        }

        private static void PrintResult(FlowAccount flowAccount)
        {
            Console.WriteLine($"Address: {flowAccount.Address.Address}");
            Console.WriteLine($"Balance: {flowAccount.Balance}");
            Console.WriteLine($"Contracts: {flowAccount.Contracts.Count}");
            Console.WriteLine($"Keys: {flowAccount.Keys.Count}\n");
            PrintKeyResult(flowAccount.Keys);
        }

        private static void PrintKeyResult(IEnumerable<FlowAccountKey> keys)
        {
            foreach(var key in keys)
            {
                Console.WriteLine($"Key Index: {key.Index}");
                Console.WriteLine($"Key Sequence Number: {key.SequenceNumber}");
                Console.WriteLine($"Key Public Key: {key.PublicKey}");
                Console.WriteLine($"Key Private Key: {key.PrivateKey}");
                Console.WriteLine($"Key Hash Algorithm: {key.HashAlgorithm}");
                Console.WriteLine($"Key Signature Algorithm: {key.SignatureAlgorithm}");
                Console.WriteLine($"Key Revoked: {key.Revoked}");
                Console.WriteLine($"Key Weight: {key.Weight}");                
            }
        }
    }
}
