using Flow.Net.Sdk.Crypto;
using Google.Protobuf;

namespace Flow.Net.Sdk.Models
{
    /// <summary>
    /// A FlowSigner is a signer associated with a specific account key.
    /// </summary>
    public class FlowSigner
    {
        public ByteString Address { get; set; }
        public uint KeyId { get; set; }
        public ISigner Signer { get; set; }
    }
}
