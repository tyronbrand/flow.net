using Google.Protobuf;
using System.Collections.Generic;

namespace Flow.Net.Sdk.Models
{
    /// <summary>
    /// A FlowAccount is an account on the Flow network.
    /// </summary>
    public class FlowAccount
    {
        public FlowAccount()
        {
            Keys = new List<FlowAccountKey>();
            Contracts = new List<FlowContract>();
        }

        public FlowAddress Address { get; set; }
        public ByteString Code { get; set; }
        public decimal Balance { get; set; }
        public IList<FlowAccountKey> Keys { get; set; }
        public IList<FlowContract> Contracts { get; }
    }
}
