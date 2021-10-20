using Newtonsoft.Json;

namespace Flow.Net.Sdk.Cadence
{
    public class CadenceBool : ICadence
    {
        public CadenceBool() {}

        public CadenceBool(bool value)
        {
            Value = value;
        }

        [JsonProperty("type")]
        public string Type => "Bool";

        [JsonProperty("value")]
        public bool Value { get; set; }
    }
}
