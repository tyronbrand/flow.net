using Google.Protobuf;
using System;
using System.Collections.Generic;

namespace Flow.Net.Sdk.Models
{
    public class FlowBlock : FlowBlockHeader
    {
        /// <summary>
        /// FlowBlock is a set of state mutations applied to the Flow blockchain.
        /// </summary>
        public FlowBlock()
        {
            BlockSeals = new List<FlowBlockSeal>();
            CollectionGuarantees = new List<FlowCollectionGuarantee>();
            Signatures = new List<ByteString>();
        }

        public IList<FlowBlockSeal> BlockSeals { get; }
        public IList<FlowCollectionGuarantee> CollectionGuarantees { get; }
        public IEnumerable<ByteString> Signatures { get; set; }
    }

    public class FlowBlockSeal
    {
        public ByteString BlockId { get; set; }
        public ByteString ExecutionReceiptId { get; set; }
        public IEnumerable<ByteString> ExecutionReceiptSignatures { get; set; }
        public IEnumerable<ByteString> ResultApprovalSignatures { get; set; }
    }

    /// <summary>
    /// A FlowCollectionGuarantee is an attestation signed by the nodes that have guaranteed a collection.
    /// </summary>
    public class FlowCollectionGuarantee
    {
        public ByteString CollectionId { get; set; }
        public IEnumerable<ByteString> Signatures { get; set; }
    }

    /// <summary>
    /// FlowBlockHeader is a summary of a full block.
    /// </summary>
    public class FlowBlockHeader
    {
        public ulong Height { get; set; }
        public ByteString Id { get; set; }
        public ByteString ParentId { get; set; }
        public DateTimeOffset Timestamp { get; set; }
    }
}
