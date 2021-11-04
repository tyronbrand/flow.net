using Newtonsoft.Json;

namespace Flow.Net.Sdk.Cadence
{
    public class CadencePath : Cadence
    {
        public CadencePath() { }

        public CadencePath(CadencePathValue value)
        {
            Value = value;
        }

        [JsonProperty("type")]
        public override string Type => "Path";

        [JsonProperty("value")]
        public CadencePathValue Value { get; set; }
    }

    public class CadencePathValue
    {
        [JsonProperty("domain")]
        public string Domain { get; set; }

        [JsonProperty("identifier")]
        public string Identifier { get; set; }
    }
}
