using System.Collections.Generic;

namespace Flow.Net.Sdk.Core.Models
{
    public class FlowTransactionResult
    {
        public FlowTransactionResult()
        {
            Events = new List<FlowEvent>();
        }

        public string BlockId { get; set; }
        public TransactionStatus Status { get; set; }
        /// <summary>Provided transaction error in case the transaction wasn't successful.</summary>
        public string ErrorMessage { get; set; }        

        public IList<FlowEvent> Events { get; set; }
        public int StatusCode { get; set; }
    }
}
