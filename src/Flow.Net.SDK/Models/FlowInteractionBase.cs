using System.Collections.Generic;
using Flow.Net.Sdk.Cadence;

namespace Flow.Net.Sdk.Models
{
    public abstract class FlowInteractionBase
    {
        public string Script { get; set; }
        public IList<ICadence> Arguments { get; set; } = new List<ICadence>();
        public Dictionary<string, string> AddressMap { get; set; } = new Dictionary<string, string>();
    }
}