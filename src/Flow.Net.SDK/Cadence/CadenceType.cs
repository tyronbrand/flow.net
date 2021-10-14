using Newtonsoft.Json;

namespace Flow.Net.Sdk.Cadence
{
    public class CadenceType : ICadence
    {
        public CadenceType() {}
        public CadenceType(FlowTypeValue value)
        {
            Value = value;
        }

        [JsonProperty("type")]
        public string Type => "Type";

        [JsonProperty("value")]
        public FlowTypeValue Value { get; set; }

        public object Decode()
        {
            return Value;
        }
    }

    public class FlowTypeValue
    {
        [JsonProperty("staticType")]
        public string StaticType { get; set; }
    }
}
