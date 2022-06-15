using Flow.Net.Sdk.Exceptions;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace Flow.Net.Sdk.Cadence.Types
{
    public class CadenceTypeConverter : CustomCreationConverter<ICadenceType>
    {
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var jObject = JObject.Load(reader);
            var target = Create(jObject);
            serializer.Populate(jObject.CreateReader(), target);

            return target;
        }

        private static ICadenceType Create(JObject jObject)
        {
            var type = (string)jObject.Property("kind");

            switch (type)
            {
                case "VariableSizedArray":
                    return new CadenceVariableSizedArrayType();
                case "ConstantSizedArray":
                    return new CadenceConstantSizedArrayType();
                case "Dictionary":
                    return new CadenceDictionaryType();
                case "Struct":
                case "Resource":
                case "Event":
                case "Contract":
                case "StructInterface":
                case "ResourceInterface":
                case "ContractInterface":
                    return new CadenceCompositeType((CadenceCompositeTypeKind)Enum.Parse(typeof(CadenceCompositeTypeKind), type));
                case "Function":
                    return new CadenceFunctionType();
                case "Reference":
                    return new CadenceReferenceType();
                case "Restriction":
                    return new CadenceRestrictedType();
                case "Capability":
                    return new CadenceCapabilityType();
                case "Enum":
                    return new CadenceEnumType();
                case "Optional":
                    return new CadenceOptionalType();
                default:
                    return new CadenceType();
            }

            throw new FlowException($"The cadence type {type} is not supported!");
        }

        public override ICadenceType Create(Type objectType)
        {
            throw new NotImplementedException();
        }
    }
}
