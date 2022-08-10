using Flow.Net.Sdk.Core.Client;
using Grpc.Net.Client;

namespace Flow.Net.Sdk.Client.Grpc
{
    public class FlowGrpcClientOptions : FlowClientOptions
    {
        public GrpcChannelOptions GrpcChannelOptions { get; set; }
    }
}
