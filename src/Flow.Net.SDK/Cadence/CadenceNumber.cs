using Newtonsoft.Json;

namespace Flow.Net.Sdk.Cadence
{
    public class CadenceNumber : Cadence
    {
        public CadenceNumber(CadenceNumberType type)
        {
            Type = type.ToString();
        }

        public CadenceNumber(CadenceNumberType type, string value)
        {
            Type = type.ToString();
            Value = value;
        }

        [JsonProperty("type")]
        public override string Type { get; }

        [JsonProperty("value")]
        public string Value { get; set; }
    }


    public enum CadenceNumberType
    {
        Int,
        UInt,
        Int8,
        UInt8,
        Int16,
        UInt16,
        Int32,
        UInt32,
        Int64,
        UInt64,
        Int128,
        UInt128,
        Int256,
        UInt256,
        Word8,
        Word16,
        Word32,
        Word64,
        Fix64,
        UFix64
    }
}
