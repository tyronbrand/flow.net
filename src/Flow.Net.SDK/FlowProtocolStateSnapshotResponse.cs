using Google.Protobuf;

namespace Flow.Net.Sdk
{
    public class FlowProtocolStateSnapshotResponse
    {
        public ByteString SerializedSnapshot { get; set; }
    }
}
