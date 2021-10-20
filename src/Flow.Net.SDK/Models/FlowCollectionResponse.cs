using Google.Protobuf;
using System.Collections.Generic;

namespace Flow.Net.Sdk.Models
{
    public class FlowCollectionResponse
    {
        public ByteString Id { get; set; }
        public IList<ByteString> TransactionIds { get; set; }
    }
}
