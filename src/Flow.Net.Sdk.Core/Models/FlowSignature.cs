namespace Flow.Net.Sdk.Core.Models
{
    /// <summary>
    /// A FlowSignature is a signature associated with a specific account key.
    /// </summary>
    public class FlowSignature
    {
        public FlowAddress Address { get; set; }
        public uint KeyId { get; set; }
        public byte[] Signature { get; set; }
    }
}
