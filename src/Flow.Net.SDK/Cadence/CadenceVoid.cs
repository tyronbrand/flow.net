using Newtonsoft.Json;

namespace Flow.Net.Sdk.Cadence
{
    public class CadenceVoid : Cadence
    {
        [JsonProperty("type")]
        public override string Type => "Void";
    }
}
