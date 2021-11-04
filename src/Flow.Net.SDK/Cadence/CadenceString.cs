using Newtonsoft.Json;

namespace Flow.Net.Sdk.Cadence
{
    public class CadenceString : Cadence
    {
        public CadenceString() { }

        public CadenceString(string value)
        {
            Value = value;
        }

        [JsonProperty("type")]
        public override string Type => "String";

        [JsonProperty("value")]
        public string Value { get; set; }
    }
}
