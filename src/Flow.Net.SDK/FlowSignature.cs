using Google.Protobuf;

namespace Flow.Net.Sdk
{
    public class FlowSignature
    {
        public ByteString Address { get; set; }
        public uint KeyId { get; set; }
        public byte[] Signature { get; set; }
    }
}
