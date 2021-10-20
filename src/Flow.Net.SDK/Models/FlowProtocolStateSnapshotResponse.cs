using Google.Protobuf;

namespace Flow.Net.Sdk.Models
{
    public class FlowProtocolStateSnapshotResponse
    {
        public ByteString SerializedSnapshot { get; set; }
    }
}
