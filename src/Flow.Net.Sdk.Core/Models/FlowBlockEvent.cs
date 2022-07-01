using System;
using System.Collections.Generic;

namespace Flow.Net.Sdk.Core.Models
{
    public class FlowBlockEvent
    {
        public ulong BlockHeight { get; set; }
        public string BlockId { get; set; }
        public DateTimeOffset BlockTimestamp { get; set; }
        public IEnumerable<FlowEvent> Events { get; set; } = new List<FlowEvent>();
    }
}
