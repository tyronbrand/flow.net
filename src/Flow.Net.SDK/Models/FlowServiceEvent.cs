using Google.Protobuf;

namespace Flow.Net.Sdk.Models
{
    public class FlowServiceEvent
    {
        public ByteString Payload { get; set; }
        public string Type { get; set; }
    }
}
