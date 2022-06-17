using Newtonsoft.Json;

namespace Flow.Net.Sdk.Cadence.Types
{
    public class CadenceOptionalType : CadenceType
    {
        public CadenceOptionalType() { }

        [JsonProperty("kind")]
        public override string Kind => "Optional";

        [JsonProperty("type")]
        public ICadenceType Type { get; set; }
    }
}
