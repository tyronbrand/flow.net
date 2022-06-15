using Newtonsoft.Json;

namespace Flow.Net.Sdk.Cadence.Types
{
    public class CadenceDictionaryType : CadenceType
    {
        public CadenceDictionaryType() { }

        public CadenceDictionaryType(ICadenceType key, ICadenceType value)
        {
            Key = key;
            Value = value;
        }

        [JsonProperty("kind")]
        public override string Kind => "Dictionary";

        [JsonProperty("key")]
        public ICadenceType Key { get; set; }

        [JsonProperty("value")]
        public ICadenceType Value { get; set; }
    }
}
