using Newtonsoft.Json;

namespace Flow.Net.Sdk.Cadence.Types
{
    public class CadenceCapabilityType : CadenceType
    {
        public CadenceCapabilityType() { }

        public CadenceCapabilityType(ICadenceType type)
        {
            Type = type;
        }

        [JsonProperty("kind")]
        public override string Kind => "Capability";

        [JsonProperty("type")]
        public ICadenceType Type { get; set; }
    }
}
