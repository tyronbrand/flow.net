using Newtonsoft.Json;
using System.Collections.Generic;

namespace Flow.Net.Sdk.Cadence
{
    public class CadenceComposite : ICadence
    {
        public CadenceComposite(string type)
        {
            Type = type;
        }

        public CadenceComposite(string type, CadenceCompositeItem value)
        {
            Type = type;
            Value = value;
        }

        [JsonProperty("type")]
        public string Type { get; set; }        

        [JsonProperty("value")]
        public CadenceCompositeItem Value { get; set; }
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
}
