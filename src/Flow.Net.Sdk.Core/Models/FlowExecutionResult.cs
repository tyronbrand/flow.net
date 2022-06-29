using System.Collections.Generic;

namespace Flow.Net.Sdk.Core.Models
{
    public class FlowExecutionResult
    {
        public string BlockId { get; set; }
        public IEnumerable<FlowChunk> Chunks { get; set; }
        public string PreviousResultId { get; set; }
        public IEnumerable<FlowServiceEvent> ServiceEvents { get; set; }
    }

    public class FlowChunk
    {
        public string BlockId { get; set; }
        public byte[] EndState { get; set; }
        public byte[] EventCollection { get; set; }
        public ulong Index { get; set; }
        public uint NumberOfTransactions { get; set; }
        public ulong TotalComputationUsed { get; set; }
        public byte[] StartState { get; set; }
    }

    public class FlowServiceEvent
    {
        public byte[] Payload { get; set; }
        public string Type { get; set; }
    }
}
