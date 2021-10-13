using Google.Protobuf;
using System.Collections.Generic;

namespace Flow.Net.Sdk.Models
{
    public class FlowAccount
    {
        public FlowAccount()
        {
            Keys = new List<FlowAccountKey>();
            Contracts = new Dictionary<string, string>();
        }

        public ByteString Address { get; set; }
        public ByteString Code { get; set; }
        public decimal Balance { get; set; }
        public IList<FlowAccountKey> Keys { get; set; }
        public IDictionary<string, string> Contracts { get; set; }
    }
}
