using Flow.Net.Sdk.Client.Grpc;
using Flow.Net.Sdk.Core.Client;
using System;
using System.Threading.Tasks;

namespace Flow.Net.Examples.TransactionExamples
{
    public class GetTransactionResultsByBlockIdExample : ExampleBase
    {
        public static async Task RunAsync(IFlowClient flowClient)
        {
            Console.WriteLine("\nRunning GetTransactionResultsByBlockId\n");
            FlowClient = flowClient;
            await PrepTransactionId();
            await Demo();
            Console.WriteLine("\nGetTransactionResultsByBlockId Complete\n");
        }

        private static async Task Demo()
        {
            if(FlowClient is FlowGrpcClient grpcClient)
            {
                var block = await grpcClient.GetLatestBlockAsync();

                var response = await grpcClient.GetTransactionResultsByBlockIdAsync(block.Header.Id);
            }
        }        

        private static async Task<string> PrepTransactionId()
        {
            return await RandomTransactionAsync();
        }
    }
}
