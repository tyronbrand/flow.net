using Newtonsoft.Json;
using System.Collections.Generic;

namespace Flow.Net.Sdk.Cadence.Types
{
    public class CadenceFunctionType : CadenceType
    {
        public CadenceFunctionType() { }

        public CadenceFunctionType(string typeId, IList<CadenceParameterType> parameters, ICadenceType @return)
        {
            TypeId = typeId;
            Parameters = parameters;
            Return = @return;
        }

        [JsonProperty("kind")]
        public override string Kind => "Function";

        [JsonProperty("typeID")]
        public string TypeId { get; set; }

        [JsonProperty("parameters")]
        public IList<CadenceParameterType> Parameters { get; set; }

        [JsonProperty("return")]
        public ICadenceType Return { get; set; }
    }
}
