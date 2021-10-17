using Google.Protobuf;

namespace Flow.Net.Sdk
{
    public class FlowServiceEvent
    {
        public ByteString Payload { get; set; }
        public string Type { get; set; }
    }
}
