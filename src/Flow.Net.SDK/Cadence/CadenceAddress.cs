using Newtonsoft.Json;

namespace Flow.Net.Sdk.Cadence
{
    public class CadenceAddress : ICadence
    {
        public CadenceAddress() { }
        public CadenceAddress(string value)
        {
            Value = value;
        }

        [JsonProperty("type")]
        public string Type => "Address";


        [JsonProperty("value")]
        public string Value { get; set; }

        public object Decode()
        {
            return Value;
        }
    }
}
