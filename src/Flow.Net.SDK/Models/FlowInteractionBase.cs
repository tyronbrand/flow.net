using Flow.Net.Sdk.Cadence;
using System.Collections.Generic;

namespace Flow.Net.Sdk.Models
{
    public abstract class FlowInteractionBase
    {
        public FlowCadenceScript Script { get; set; }
        public IList<ICadence> Arguments { get; set; } = new List<ICadence>();        
    }
}