namespace Flow.Net.Sdk.Core.Models
{
    /// <summary>
    /// A FlowProposalKey is the key that specifies the proposal key and sequence number for a transaction.
    /// </summary>
    public class FlowProposalKey
    {
        public FlowAddress Address { get; set; }
        public uint KeyId { get; set; }
        public ulong SequenceNumber { get; set; }
    }
}
