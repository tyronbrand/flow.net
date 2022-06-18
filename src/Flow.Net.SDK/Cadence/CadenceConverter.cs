using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using System;
using Flow.Net.Sdk.Exceptions;

namespace Flow.Net.Sdk.Cadence
{
    public class CadenceConverter : CustomCreationConverter<ICadence>
    {
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var jObject = JObject.Load(reader);
            var target = Create(jObject);

            switch (target.Type)
            {
                case "Optional":
                    {
                        var value = jObject.Property("value");
                        if (value.Value.Type == JTokenType.Null)
                            return new CadenceOptional();
                    }
                    break;
            }

            serializer.Populate(jObject.CreateReader(), target);

            return target;
        }

        private static ICadence Create(JObject jObject)
        {
            var type = (string)jObject.Property("type");

            switch (type)
            {
                case "String":
                    return new CadenceString();
                case "Array":
                    return new CadenceArray();
                case "Bool":
                    return new CadenceBool();
                case "Address":
                    return new CadenceAddress();
                case "Void":
                    return new CadenceVoid();
                case "Dictionary":
                    return new CadenceDictionary();
                case "Capability":
                    return new CadenceCapability();
                case "Type":
                    return new CadenceTypeValue();
                case "Optional":
                    return new CadenceOptional();
                case "Path":
                    return new CadencePath();
                case "Link":
                    return new CadenceLink();
                case "Struct":
                case "Resource":
                case "Event":
                case "Contract":
                case "Enum":
                    return new CadenceComposite((CadenceCompositeType)Enum.Parse(typeof(CadenceCompositeType), type));
                case "Int":
                case "UInt":
                case "Int8":
                case "UInt8":
                case "Int16":
                case "UInt16":
                case "Int32":
                case "UInt32":
                case "Int64":
                case "UInt64":
                case "Int128":
                case "UInt128":
                case "Int256":
                case "UInt256":
                case "Word8":
                case "Word16":
                case "Word32":
                case "Word64":
                case "Fix64":
                case "UFix64":
                    return new CadenceNumber((CadenceNumberType)Enum.Parse(typeof(CadenceNumberType), type));
            }

            throw new FlowException($"The type {type} is not supported!");
        }

        public override ICadence Create(Type objectType)
        {
            throw new NotImplementedException();
        }
    }
}
