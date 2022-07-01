using System.Collections.Generic;

namespace Flow.Net.Sdk.Core.Models
{
    /// <summary>
    /// A FlowAccount is an account on the Flow network.
    /// </summary>
    public class FlowAccount
    {
        public FlowAddress Address { get; set; }
        public string Code { get; set; }
        public decimal Balance { get; set; }
        public IList<FlowAccountKey> Keys { get; set; } = new List<FlowAccountKey>();
        public IList<FlowContract> Contracts { get; set; } = new List<FlowContract>();
    }
}
