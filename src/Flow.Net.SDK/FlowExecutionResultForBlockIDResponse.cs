using Google.Protobuf;
using System.Collections.Generic;

namespace Flow.Net.Sdk
{
    public class FlowExecutionResultForBlockIDResponse
    {
        public ByteString BlockId { get; set; }        
        public IEnumerable<FlowChunk> Chunks { get; set; }
        public ByteString PreviousResultId { get; set; }
        public IEnumerable<FlowServiceEvent> ServiceEvents { get; set; }
    }
}
