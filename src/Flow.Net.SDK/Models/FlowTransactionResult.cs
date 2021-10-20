using Flow.Net.Sdk.Protos.entities;
using Google.Protobuf;
using System.Collections.Generic;

namespace Flow.Net.Sdk.Models
{
    public class FlowTransactionResult
    {
        public FlowTransactionResult()
        {
            Events = new List<FlowEvent>();
        }

        public ByteString BlockId { get; set; }
        public string ErrorMessage { get; set; }
        public IList<FlowEvent> Events { get; set; }
        public TransactionStatus Status { get; set; }
        public uint StatusCode { get; set; }
    }
}
