using Newtonsoft.Json;

namespace Flow.Net.Sdk.Core.Cadence
{
    public class CadenceAddress : Cadence
    {
        public CadenceAddress() { }
        public CadenceAddress(string value)
        {
            Value = null; //value.AddHexPrefix();
        }

        [JsonProperty("type")]
        public override string Type => "Address";


        [JsonProperty("value")]
        public string Value { get; set; }
    }
}
