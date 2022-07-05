using Newtonsoft.Json;

namespace Flow.Net.Sdk.Core.Cadence.Types
{
    public class CadenceFieldType
    {
        public CadenceFieldType() { }

        public CadenceFieldType(string id, ICadenceType type)
        {
            Id = id;
            Type = type;
        }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("type")]
        public ICadenceType Type { get; set; }
    }
}
