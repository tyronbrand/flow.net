using Newtonsoft.Json;

namespace Flow.Net.Sdk.Cadence
{
    public class CadenceAddress : Cadence
    {
        public CadenceAddress() { }
        public CadenceAddress(string value)
        {
            Value = value;
        }

        [JsonProperty("type")]
        public override string Type => "Address";


        [JsonProperty("value")]
        public string Value { get; set; }
    }
}
