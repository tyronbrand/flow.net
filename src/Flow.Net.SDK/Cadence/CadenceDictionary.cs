using Newtonsoft.Json;
using System.Collections.Generic;

namespace Flow.Net.Sdk.Cadence
{
    public class CadenceDictionary : ICadence
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
        public string Type => "Dictionary";

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
