using Google.Protobuf;
using System.Collections.Generic;

namespace Flow.Net.Sdk.Models
{
    public class FlowTransaction : FlowTransactionBase
    {
        public FlowTransaction()
        {
            SignerList = new Dictionary<ByteString, int>();
        }

        public Dictionary<ByteString, int> SignerList { get; set; }
    }    
}
