using Flow.Net.Sdk.Cadence;
using Google.Protobuf;

namespace Flow.Net.Sdk.Models
{
    public class FlowEvent
    {        
        public uint EventIndex { get; set; }
        public ICadence Payload { get; set; }
        public ByteString TransactionId { get; set; }
        public uint TransactionIndex { get; set; }
        public string Type { get; set; }
    }
}
