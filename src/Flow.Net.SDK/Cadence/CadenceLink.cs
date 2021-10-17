using Newtonsoft.Json;

namespace Flow.Net.Sdk.Cadence
{
    public class CadenceLink : ICadence
    {
        public CadenceLink() { }
        public CadenceLink(CadenceLinkValue value)
        {
            Value = value;
        }

        [JsonProperty("type")]
        public string Type => "Link";

        [JsonProperty("value")]
        public CadenceLinkValue Value { get; set; }
    }

    public class CadenceLinkValue
    {
        [JsonProperty("targetPath")]
        public CadencePath TargetPath { get; set; }

        [JsonProperty("borrowType")]
        public string BorrowType { get; set; }
    }
}
