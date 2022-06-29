using Newtonsoft.Json;

namespace Flow.Net.Sdk.Core.Cadence
{
    public class CadenceBool : Cadence
    {
        public CadenceBool() {}

        public CadenceBool(bool value)
        {
            Value = value;
        }

        [JsonProperty("type")]
        public override string Type => "Bool";

        [JsonProperty("value")]
        public bool Value { get; set; }
    }
}
