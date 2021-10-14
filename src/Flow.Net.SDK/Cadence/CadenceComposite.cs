using Newtonsoft.Json;
using System.Collections.Generic;
using System.Dynamic;
using System.Text;

namespace Flow.Net.Sdk.Cadence
{
    public class CadenceComposite : ICadence
    {
        public CadenceComposite(CadenceCompositeType type)
        {
            Type = type.ToString();
        }

        public CadenceComposite(CadenceCompositeType type, CadenceCompositeItem value)
        {
            Type = type.ToString();
            Value = value;
        }

        [JsonProperty("type")]
        public string Type { get; set; }        

        [JsonProperty("value")]
        public CadenceCompositeItem Value { get; set; }

        public object Decode()
        {            
            return Value;
        }
    }

    public class CadenceCompositeItem
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("fields")]
        public IEnumerable<CadenceCompositeItemField> Fields { get; set; }        
    }

    public class CadenceCompositeItemField
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("value")]
        public ICadence Value { get; set; }
    }

    public enum CadenceCompositeType
    {
        Struct,
        Resource,
        Event,
        Contract,
        Enum
    }
}
