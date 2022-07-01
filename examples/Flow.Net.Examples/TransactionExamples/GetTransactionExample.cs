using Flow.Net.Sdk.Core.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Flow.Net.Examples.TransactionExamples
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

        private static async Task Demo(string transactionId)
        {
            var tx = await FlowClient.GetTransactionAsync(transactionId);
            PrintTransaction(tx);

            var txr = await FlowClient.GetTransactionResultAsync(transactionId);
            PrintTransactionResult(txr);
        }

        private static void PrintTransaction(FlowTransactionBase tx)
        {
            Console.WriteLine($"ReferenceBlockId: {tx.ReferenceBlockId}");
            Console.WriteLine($"Payer: {tx.Payer.Address}");
            Console.WriteLine("Authorizers: [{0}]", string.Join(", ", tx.Authorizers.Select(s => s.Address).ToArray()));
            Console.WriteLine($"Proposer: {tx.ProposalKey.Address.Address}");
        }

        private static void PrintTransactionResult(FlowTransactionResult txr)
        {
            Console.WriteLine($"Status: {txr.Status}");
            Console.WriteLine($"Error: {txr.ErrorMessage}\n");
        }

        private static async Task<string> PrepTransactionId()
        {
            return await RandomTransactionAsync();
        }
    }
}
