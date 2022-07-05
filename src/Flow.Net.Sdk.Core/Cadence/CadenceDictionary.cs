using Newtonsoft.Json;
using System.Collections.Generic;

namespace Flow.Net.Sdk.Core.Cadence
{
    public class CadenceDictionary : Cadence
    {
        public CadenceDictionary() 
        {
            Value = new List<CadenceDictionaryKeyValue>();
        }

        public CadenceDictionary(IList<CadenceDictionaryKeyValue> value)
        {
            Value = value;
        }

        [JsonProperty("type")]
        public override string Type => "Dictionary";

        [JsonProperty("value")]
        public IList<CadenceDictionaryKeyValue> Value { get; set; }
    }

    public class CadenceDictionaryKeyValue
    {
        [JsonProperty("key")]
        public ICadence Key { get; set; }

        [JsonProperty("value")]
        public ICadence Value { get; set; }
    }    
}
