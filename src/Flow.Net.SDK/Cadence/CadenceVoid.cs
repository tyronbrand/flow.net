using Newtonsoft.Json;

namespace Flow.Net.Sdk.Cadence
{
    public class CadenceVoid : ICadence
    {
        [JsonProperty("type")]
        public string Type => "Void";
    }
}
