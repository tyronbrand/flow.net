using Newtonsoft.Json;
using System.Collections.Generic;

namespace Flow.Net.Sdk.Core.Cadence
{
    public class CadenceArray : Cadence
    {
        public CadenceArray()
        {
            Value = new List<ICadence>();
        }

        public CadenceArray(IList<ICadence> value)
        {
            Value = value;
        }

        [JsonProperty("type")]
        public override string Type => "Array";

        [JsonProperty("value")]
        public IList<ICadence> Value { get; set; }
    }
}
