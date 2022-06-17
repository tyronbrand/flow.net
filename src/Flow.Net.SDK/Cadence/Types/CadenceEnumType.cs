using Newtonsoft.Json;
using System.Collections.Generic;

namespace Flow.Net.Sdk.Cadence.Types
{
    public class CadenceEnumType : CadenceType
    {
        public CadenceEnumType() { }

        public CadenceEnumType(string typeId, ICadenceType type)
        {
            TypeId = typeId;
            Type = type;
        }

        [JsonProperty("kind")]
        public override string Kind => "Enum";

        [JsonProperty("type")]
        public ICadenceType Type { get; set; }

        [JsonProperty("typeID")]
        public string TypeId { get; set; }

        [JsonProperty("initializers")]
        public IList<CadenceInitializerType> Initializers { get; } = new List<CadenceInitializerType>();

        [JsonProperty("fields")]
        public IList<CadenceFieldType> Fields { get; set; }
    }
}
