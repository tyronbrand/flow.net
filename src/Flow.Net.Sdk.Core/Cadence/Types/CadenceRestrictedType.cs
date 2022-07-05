using Newtonsoft.Json;
using System.Collections.Generic;

namespace Flow.Net.Sdk.Core.Cadence.Types
{
    public class CadenceRestrictedType : CadenceType
    {
        public CadenceRestrictedType() { }

        public CadenceRestrictedType(string typeId, ICadenceType type, IList<ICadenceType> restrictions)
        {
            TypeId = typeId;
            Type = type;
            Restrictions = restrictions;
        }

        [JsonProperty("kind")]
        public override string Kind => "Restriction";

        [JsonProperty("typeID")]
        public string TypeId { get; set; }

        [JsonProperty("type")]
        public ICadenceType Type { get; set; }

        [JsonProperty("restrictions")]
        public IList<ICadenceType> Restrictions { get; set; }
    }
}
