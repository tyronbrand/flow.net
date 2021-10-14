using Newtonsoft.Json;
using System;
using System.Numerics;

namespace Flow.Net.Sdk.Cadence
{
    public class CadenceNumber : ICadence
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
        public string Type { get; set; }

        [JsonProperty("value")]
        public string Value { get; set; }

        public object Decode()
        {
            switch (Type)
            {               
                case "Int":
                    return Convert.ToSByte(Value);
                case "UInt":
                    return Convert.ToByte(Value);
                case "Int8":
                    return Convert.ToSByte(Value);
                case "UInt8":
                case "Word8":
                    return Convert.ToByte(Value);
                case "Int16":
                    return Convert.ToInt16(Value);
                case "UInt16":
                case "Word16":
                    return Convert.ToUInt16(Value);
                case "Int32":
                    return Convert.ToInt32(Value);
                case "UInt32":
                case "Word32":
                    return Convert.ToUInt32(Value);
                case "Int64":
                    return Convert.ToInt64(Value);
                case "UInt64":
                case "Word64":
                    return Convert.ToUInt64(Value);
                case "Int128":
                case "UInt128":
                case "Int256":
                case "UInt256":
                    return BigInteger.Parse(Value.ToString());
                case "Fix64":
                    return Convert.ToDecimal(Value);
                case "UFix64":
                    return Convert.ToDecimal(Value);
            }

            return null;
        }
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
