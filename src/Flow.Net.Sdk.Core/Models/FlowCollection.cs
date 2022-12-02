using System.Collections.Generic;

namespace Flow.Net.Sdk.Core.Models
{
    public class FlowCollection
    {
        public string Id { get; set; }
        public IList<FlowTransactionId> TransactionIds { get; set; } = new List<FlowTransactionId>();
        public IList<FlowTransactionResponse> Transactions { get; set; } = new List<FlowTransactionResponse>();
    }
}
