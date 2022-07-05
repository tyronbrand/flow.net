using Newtonsoft.Json;

namespace Flow.Net.Sdk.Core.Cadence.Types
{
    public class CadenceParameterType
    {
        public CadenceParameterType() { }

        public CadenceParameterType(string label, string id, ICadenceType type)
        {
            Label = label;
            Id = id;
            Type = type;
        }

        [JsonProperty("label")]
        public string Label { get; set; }
        
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("type")]
        public ICadenceType Type { get; set; }
    }
}
