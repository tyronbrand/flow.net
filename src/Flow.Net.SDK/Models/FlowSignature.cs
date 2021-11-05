using Google.Protobuf;

namespace Flow.Net.Sdk.Models
{
    /// <summary>
    /// A FlowSignature is a signature associated with a specific account key.
    /// </summary>
    public class FlowSignature
    {
        public ByteString Address { get; set; }
        public uint KeyId { get; set; }
        public byte[] Signature { get; set; }
    }
}
