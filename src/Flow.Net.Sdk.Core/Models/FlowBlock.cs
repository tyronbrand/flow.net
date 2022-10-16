using System;
using System.Collections.Generic;

namespace Flow.Net.Sdk.Core.Models
{
    public class FlowBlock
    {
        public FlowBlockHeader Header { get; set; } = new FlowBlockHeader();
        public FlowBlockPayload Payload { get; set; } = new FlowBlockPayload();
    }

    public class FlowBlockPayload
    {
        public IEnumerable<FlowCollectionGuarantee> CollectionGuarantees { get; set; } = new List<FlowCollectionGuarantee>();
        public IEnumerable<FlowBlockSeal> Seals { get; set; } = new List<FlowBlockSeal>();
    }

    public class FlowBlockSeal
    {        
        public string BlockId { get; set; }
        public string ResultId { get; set; }
    }

    public class FlowCollectionGuarantee
    {
        public string CollectionId { get; set; }
        public byte[] Signature { get; set; }
        public IEnumerable<string> SignerIds { get; set; }
    }

    public class FlowBlockHeader
    {
        public string Id { get; set; }        
        public string ParentId { get; set; }        
        public ulong Height { get; set; }       
        public DateTimeOffset Timestamp { get; set; }
        public byte[] ParentVoterSignature { get; set; }
    }
}
