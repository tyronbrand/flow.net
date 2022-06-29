using Newtonsoft.Json;
using System.Collections.Generic;

namespace Flow.Net.Sdk.Core.Cadence.Types
{
    public class CadenceEnumType : CadenceType
    {
        public CadenceEnumType() { }

        public CadenceEnumType(string typeId, ICadenceType type, IList<CadenceFieldType> fields)
        {
            TypeId = typeId;
            Type = type;
            Fields = fields;
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
        public IList<CadenceFieldType> Fields { get; set; } = new List<CadenceFieldType>();
    }
}
