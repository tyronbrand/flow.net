using Newtonsoft.Json;

namespace Flow.Net.Sdk.Cadence
{
    public class CadenceOptional : Cadence
    {
        public CadenceOptional() { }

        public CadenceOptional(ICadence value)
        {
            Value = value;
        }

        [JsonProperty("type")]
        public override string Type => "Optional";

        [JsonProperty("value")]
        public ICadence Value { get; set; }
    }
}
