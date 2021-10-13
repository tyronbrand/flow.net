using Google.Protobuf;
using System;
using System.Collections.Generic;

namespace Flow.Net.Sdk.Models
{
    public class FlowBlock : FlowBlockHeader
    {
        public FlowBlock()
        {
            BlockSeals = new List<FlowBlockSeal>();
            CollectionGuarantees = new List<FlowCollectionGuarantee>();
            Signatures = new List<ByteString>();
        }

        public IList<FlowBlockSeal> BlockSeals { get; set; }
        public IList<FlowCollectionGuarantee> CollectionGuarantees { get; set; }
        public IEnumerable<ByteString> Signatures { get; set; }
    }

    public class FlowBlockSeal
    {
        public ByteString BlockId { get; set; }
        public ByteString ExecutionReceiptId { get; set; }
        public IEnumerable<ByteString> ExecutionReceiptSignatures { get; set; }
        public IEnumerable<ByteString> ResultApprovalSignatures { get; set; }
    }

    public class FlowCollectionGuarantee
    {
        public ByteString CollectionId { get; set; }
        public IEnumerable<ByteString> Signatures { get; set; }
    }

    public class FlowBlockHeader
    {
        public ulong Height { get; set; }
        public ByteString Id { get; set; }
        public ByteString ParentId { get; set; }
        public DateTimeOffset Timestamp { get; set; }
    }
}
