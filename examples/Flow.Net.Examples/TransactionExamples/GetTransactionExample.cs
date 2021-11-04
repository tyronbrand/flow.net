using Flow.Net.Sdk;
using Flow.Net.Sdk.Models;
using Google.Protobuf;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Flow.Net.Examples
{
    public class GetTransactionExample : ExampleBase
    {
        public static async Task RunAsync()
        {
            Console.WriteLine("\nRunning GetTransactionExample\n");
            await CreateFlowClientAsync();
            var txId = await PrepTransactionId();
            await Demo(txId);
            Console.WriteLine("\nGetTransactionExample Complete\n");
        }

        private static async Task Demo(ByteString transactionId)
        {
            var tx = await FlowClient.GetTransactionAsync(transactionId);
            PrintTransaction(tx);

            var txr = await FlowClient.GetTransactionResultAsync(transactionId);
            PrintTransactionResult(txr);
        }

        private static void PrintTransaction(FlowTransactionResponse tx)
        {
            Console.WriteLine($"ReferenceBlockId: {tx.ReferenceBlockId.FromByteStringToHex()}");
            Console.WriteLine($"Payer: {tx.Payer.FromByteStringToHex()}");
            Console.WriteLine("Authorizers: [{0}]", string.Join(", ", tx.Authorizers.Select(s => s.FromByteStringToHex()).ToArray()));
            Console.WriteLine($"Proposer: {tx.ProposalKey.Address.FromByteStringToHex()}");
        }

        private static void PrintTransactionResult(FlowTransactionResult txr)
        {
            Console.WriteLine($"Status: {txr.Status}");
            Console.WriteLine($"Error: {txr.ErrorMessage}\n");            
        }

        private static async Task<ByteString> PrepTransactionId()
        {
            return await RandomTransactionAsync();
        }
    }
}
