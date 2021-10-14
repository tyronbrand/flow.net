using Newtonsoft.Json;

namespace Flow.Net.Sdk.Cadence
{
    public class CadenceVoid : ICadence
    {
        [JsonProperty("type")]
        public string Type => "Void";

        private string _decodeValue { get; set; }
        public object Decode()
        {
            _decodeValue = null;
            return _decodeValue;
        }
    }
}
