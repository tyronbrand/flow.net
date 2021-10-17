using Google.Protobuf;

namespace Flow.Net.Sdk
{
    public class FlowChunk
    {
        public ByteString BlockId { get; set; }
        public ByteString EndState { get; set; }
        public ByteString EventCollection { get; set; }        
        public ulong Index { get; set; }
        public ulong NumberOfTransactions { get; set; }
        public ulong TotalComputationUsed { get; set; }
        public ByteString StartState { get; set; }
    }
}
