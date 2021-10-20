using Google.Protobuf;
using System.Collections.Generic;

namespace Flow.Net.Sdk.Models
{
    public abstract class FlowTransactionBase
    {
        public FlowTransactionBase()
        {
            Arguments = new List<ByteString>();
            Authorizers = new List<ByteString>();
            PayloadSignatures = new List<FlowSignature>();
            EnvelopeSignatures = new List<FlowSignature>();
            GasLimit = 9999;
        }

        public string Script { get; set; }
        public IList<ByteString> Arguments { get; set; }
        public ByteString ReferenceBlockId { get; set; }
        public ulong GasLimit { get; set; }
        public ByteString Payer { get; set; }
        public FlowProposalKey ProposalKey { get; set; }
        public IList<ByteString> Authorizers { get; set; }
        public IList<FlowSignature> PayloadSignatures { get; set; }
        public IList<FlowSignature> EnvelopeSignatures { get; set; }
    }
}
