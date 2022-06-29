using Newtonsoft.Json;

namespace Flow.Net.Sdk.Core.Cadence.Types
{
    public class CadenceVariableSizedArrayType : CadenceType
    {
        public CadenceVariableSizedArrayType() { }

        public CadenceVariableSizedArrayType(ICadenceType type)
        {
            Type = type;
        }

        [JsonProperty("kind")]
        public override string Kind => "VariableSizedArray";

        [JsonProperty("type")]
        public ICadenceType Type { get; set; }
    }
}
