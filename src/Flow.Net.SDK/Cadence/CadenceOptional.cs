using Newtonsoft.Json;

namespace Flow.Net.Sdk.Cadence
{
    public class CadenceOptional : ICadence
    {
        public CadenceOptional() {}

        public CadenceOptional(ICadence value)
        {
            Value = value;
        }

        [JsonProperty("type")]
        public string Type => "Optional";

        [JsonProperty("value")]
        public ICadence Value { get; set; }

        private string _decodeValue { get; set; }
        public object Decode()
        {
            if(Value == null)
            {
                _decodeValue = null;
                return _decodeValue;
            }

            return Value.Decode();
        }

    }
}
