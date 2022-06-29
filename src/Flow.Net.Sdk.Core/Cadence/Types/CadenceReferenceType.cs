using Newtonsoft.Json;

namespace Flow.Net.Sdk.Core.Cadence.Types
{
    public class CadenceReferenceType : CadenceType
    {
        public CadenceReferenceType() { }

        public CadenceReferenceType(bool authorized, ICadenceType type)
        {
            Authorized = authorized;
            Type = type;
        }

        [JsonProperty("kind")]
        public override string Kind => "Reference";

        [JsonProperty("authorized")]
        public bool Authorized { get; set; }

        [JsonProperty("type")]
        public ICadenceType Type { get; set; }
    }
}
