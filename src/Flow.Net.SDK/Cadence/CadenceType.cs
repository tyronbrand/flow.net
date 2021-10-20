using Newtonsoft.Json;

namespace Flow.Net.Sdk.Cadence
{
    public class CadenceType : ICadence
    {
        public CadenceType() { }

        public CadenceType(CadenceTypeValue value)
        {
            Value = value;
        }

        [JsonProperty("type")]
        public string Type => "Type";

        [JsonProperty("value")]
        public CadenceTypeValue Value { get; set; }
    }

    public class CadenceTypeValue
    {
        [JsonProperty("staticType")]
        public string StaticType { get; set; }
    }
}
