using Flow.Net.Sdk.Cadence;
using Google.Protobuf;
using System.Collections.Generic;

namespace Flow.Net.Sdk.Models
{
    public abstract class FlowTransactionBase
    {
        protected FlowTransactionBase()
        {
            Arguments = new List<ICadence>();
            Authorizers = new List<FlowAddress>();
            PayloadSignatures = new List<FlowSignature>();
            EnvelopeSignatures = new List<FlowSignature>();
            GasLimit = 9999;
        }

        public string Script { get; set; }
        public IList<ICadence> Arguments { get; set; }
        public ByteString ReferenceBlockId { get; set; }
        public ulong GasLimit { get; set; }
        public FlowAddress Payer { get; set; }
        public FlowProposalKey ProposalKey { get; set; }
        public IList<FlowAddress> Authorizers { get; set; }
        public IList<FlowSignature> PayloadSignatures { get; set; }
        public IList<FlowSignature> EnvelopeSignatures { get; set; }
        public Dictionary<string, string> AddressMap { get; set; } = new Dictionary<string, string>();
    }
}
