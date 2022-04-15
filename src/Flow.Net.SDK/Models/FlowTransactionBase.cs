using Google.Protobuf;
using System.Collections.Generic;

namespace Flow.Net.Sdk.Models
{
    public abstract class FlowTransactionBase : FlowInteractionBase
    {
        protected FlowTransactionBase(Dictionary<string, string> addressMap = null) 
            : base(addressMap)
        {
            Authorizers = new List<FlowAddress>();
            PayloadSignatures = new List<FlowSignature>();
            EnvelopeSignatures = new List<FlowSignature>();
            GasLimit = 9999;
        }

        public ByteString ReferenceBlockId { get; set; }
        public ulong GasLimit { get; set; }
        public FlowAddress Payer { get; set; }
        public FlowProposalKey ProposalKey { get; set; }
        public IList<FlowAddress> Authorizers { get; set; }
        public IList<FlowSignature> PayloadSignatures { get; set; }
        public IList<FlowSignature> EnvelopeSignatures { get; set; }
    }
}
