using Newtonsoft.Json;

namespace Flow.Net.Sdk.Core.Cadence
{
    public class CadenceVoid : Cadence
    {
        [JsonProperty("type")]
        public override string Type => "Void";
    }
}
