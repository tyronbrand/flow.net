using Newtonsoft.Json;

namespace Flow.Net.Sdk.Core.Cadence.Types
{
    public class CadenceConstantSizedArrayType : CadenceType
    {
        public CadenceConstantSizedArrayType() { }

        public CadenceConstantSizedArrayType(ICadenceType type, long size)
        {
            Type = type;
            Size = size;
        }

        [JsonProperty("kind")]
        public override string Kind => "ConstantSizedArray";

        [JsonProperty("type")]
        public ICadenceType Type { get; set; }

        [JsonProperty("size")]
        public long Size { get; set; }
    }
}
