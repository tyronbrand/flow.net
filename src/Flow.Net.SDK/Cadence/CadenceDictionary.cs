using Newtonsoft.Json;
using System.Collections.Generic;

namespace Flow.Net.Sdk.Cadence
{
    public class CadenceDictionary : ICadence
    {
        public CadenceDictionary() 
        {
            Value = new List<FlowDictionaryItem>();
        }
        public CadenceDictionary(IEnumerable<FlowDictionaryItem> value)
        {
            Value = value;
        }

        [JsonProperty("type")]
        public string Type => "Dictionary";

        [JsonProperty("value")]
        public IEnumerable<FlowDictionaryItem> Value { get; set; }
    }

    public class FlowDictionaryItem
    {
        [JsonProperty("key")]
        public ICadence Key { get; set; }

        [JsonProperty("value")]
        public ICadence Value { get; set; }
    }
}
