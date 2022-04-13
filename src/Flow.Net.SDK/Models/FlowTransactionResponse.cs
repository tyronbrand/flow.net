using System.Collections.Generic;

namespace Flow.Net.Sdk.Models
{
    public class FlowTransactionResponse : FlowTransactionBase
    {
        public IList<FlowSignature> PayloadSignatures { get; set; } = new List<FlowSignature>();
        public IList<FlowSignature> EnvelopeSignatures { get; set; } = new List<FlowSignature>();
    }
}
