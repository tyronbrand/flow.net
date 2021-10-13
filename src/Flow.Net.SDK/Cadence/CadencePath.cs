using Newtonsoft.Json;

namespace Flow.Net.Sdk.Cadence
{
    public class CadencePath : ICadence
    {
        public CadencePath() {}
        public CadencePath(FlowPathValue value)
        {
            Value = value;
        }

        [JsonProperty("type")]
        public string Type => "Path";

        [JsonProperty("value")]
        public FlowPathValue Value { get; set; }
    }

    public class FlowPathValue
    {
        [JsonProperty("domain")]
        public string Domain { get; set; }

        [JsonProperty("identifier")]
        public string Identifier { get; set; }
    }
}
