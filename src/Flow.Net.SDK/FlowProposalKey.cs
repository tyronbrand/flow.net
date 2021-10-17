using Google.Protobuf;

namespace Flow.Net.Sdk
{
    public class FlowProposalKey
    {
        public ByteString Address { get; set; }
        public uint KeyId { get; set; }
        public ulong SequenceNumber { get; set; }
    }
}
