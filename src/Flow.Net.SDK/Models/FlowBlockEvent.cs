using Google.Protobuf;
using System;
using System.Collections.Generic;

namespace Flow.Net.Sdk.Models
{
    public class FlowBlockEvent
    {
        public FlowBlockEvent()
        {
            Events = new List<FlowEvent>();
        }

        public ulong BlockHeight { get; set; }
        public ByteString BlockId { get; set; }
        public DateTimeOffset BlockTimestamp { get; set; }
        public IList<FlowEvent> Events { get; set; }
    }
}
