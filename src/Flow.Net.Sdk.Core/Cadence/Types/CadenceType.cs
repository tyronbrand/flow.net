using Newtonsoft.Json;

namespace Flow.Net.Sdk.Core.Cadence.Types
{
    public class CadenceType : ICadenceType
    {
        [JsonProperty("kind")]
        public virtual string Kind { get; set; }

        /// <summary>
        /// Encodes <see cref="ICadenceType"/>.
        /// </summary>
        /// <param name="cadenceType"></param>
        /// <returns>A JSON string representation of <see cref="ICadenceType"/>.</returns>
        public string Encode(ICadenceType cadenceType)
        {
            JsonConverter[] jsonConverters = { new CadenceRepeatedTypeConverter(), new CadenceTypeValueAsStringConverter() };
            return JsonConvert.SerializeObject(cadenceType, jsonConverters);
        }
    }
}
