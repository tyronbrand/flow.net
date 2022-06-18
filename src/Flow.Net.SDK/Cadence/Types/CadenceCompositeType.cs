using Newtonsoft.Json;
using System.Collections.Generic;

namespace Flow.Net.Sdk.Cadence.Types
{
    public class CadenceCompositeType : CadenceType
    {
        public CadenceCompositeType(CadenceCompositeTypeKind kind)
        {
            Kind = kind.ToString();
        }

        public CadenceCompositeType(CadenceCompositeTypeKind kind, string typeId, IList<IList<CadenceInitializerType>> initializers, IList<CadenceFieldType> fields)
        {
            Kind = kind.ToString();
            TypeId = typeId;
            Initializers = initializers;
            Fields = fields;
        }

        [JsonProperty("kind")]
        public sealed override string Kind { get; set; }

        [JsonProperty("type")]
        public string Type { get; } = "";

        [JsonProperty("typeID")]
        public string TypeId { get; set; }

        [JsonProperty("initializers")]
        public IList<IList<CadenceInitializerType>> Initializers { get; set; } = new List<IList<CadenceInitializerType>>();

        [JsonProperty("fields")]
        public IList<CadenceFieldType> Fields { get; set; } = new List<CadenceFieldType>();

    }

    public enum CadenceCompositeTypeKind
    {
        Struct,
        Resource,
        Event,
        Contract,
        StructInterface,
        ResourceInterface,
        ContractInterface
    }
}
