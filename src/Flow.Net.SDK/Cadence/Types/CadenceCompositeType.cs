using Newtonsoft.Json;
using System.Collections.Generic;

namespace Flow.Net.Sdk.Cadence.Types
{
    public class CadenceCompositeType : CadenceType
    {
        public CadenceCompositeType(CadenceCompositeTypeKind kind)
        {
            Kind = Kind.ToString();
        }

        public CadenceCompositeType(CadenceCompositeTypeKind kind, string typeId, IList<CadenceInitializerType> initializers, IList<CadenceFieldType> fields)
        {
            Kind = kind.ToString();
            TypeId = typeId;
            Initializers = initializers;
            Fields = fields;
        }


        [JsonProperty("kind")]
        public sealed override string Kind { get; set; }

        [JsonProperty("type")]
        public string Type { get; }

        [JsonProperty("typeID")]
        public string TypeId { get; set; }

        [JsonProperty("initializers")]
        public IList<CadenceInitializerType> Initializers { get; set; }

        [JsonProperty("fields")]
        public IList<CadenceFieldType> Fields { get; set; }

    }

    public enum CadenceCompositeTypeKind
    {
        Struct,
        Resource,
        Event,
        Constract,
        StructInterface,
        ResourceInterface,
        ContractInterface
    }
}
