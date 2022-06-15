using Flow.Net.Sdk.Cadence.Types;
using Newtonsoft.Json;

namespace Flow.Net.Sdk.Cadence
{
    public class CadenceTypeValue : Cadence
    {
        public CadenceTypeValue() { }

        public CadenceTypeValue(CadenceTypeValueValue value)
        {
            Value = value;
        }

        [JsonProperty("type")]
        public override string Type => "Type";

        [JsonProperty("value")]
        public CadenceTypeValueValue Value { get; set; }
    }

    public class CadenceTypeValueValue
    {
        [JsonProperty("staticType")]
        public ICadenceType StaticType { get; set; }
    }
}
