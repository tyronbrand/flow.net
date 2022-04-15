using System.Collections.Generic;

namespace Flow.Net.Sdk.Models
{
    public class FlowScript : FlowInteractionBase 
    {
        public FlowScript(Dictionary<string, string> addressMap = null)
            : base(addressMap)
        {}
    }
}