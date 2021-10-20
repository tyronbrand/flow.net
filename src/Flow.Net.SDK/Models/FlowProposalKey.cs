using Google.Protobuf;

namespace Flow.Net.Sdk.Models
{
    public class FlowProposalKey
    {
        public ByteString Address { get; set; }
        public uint KeyId { get; set; }
        public ulong SequenceNumber { get; set; }
    }
}
