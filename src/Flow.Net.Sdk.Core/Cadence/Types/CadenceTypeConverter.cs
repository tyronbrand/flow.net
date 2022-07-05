using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace Flow.Net.Sdk.Core.Cadence.Types
{
    public class CadenceTypeConverter : CustomCreationConverter<ICadenceType>
    {
        private static readonly Dictionary<string, ICadenceType> _compositeDictionary = new Dictionary<string, ICadenceType>();

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.Path.Contains("type") && reader.TokenType == JsonToken.String && _compositeDictionary.TryGetValue((string)reader.Value, out var composite))
                return composite;

            var jObject = JObject.Load(reader);
            var target = Create(jObject);
            HandleCadenceCompositeType(jObject, target);
            serializer.Populate(jObject.CreateReader(), target);
            return target;            
        }

        private static ICadenceType Create(JObject jObject)
        {
            var kind = (string)jObject.Property("kind");

            switch (kind)
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
                    return new CadenceCompositeType((CadenceCompositeTypeKind)Enum.Parse(typeof(CadenceCompositeTypeKind), kind));
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
        }

        private static void HandleCadenceCompositeType(JObject jObject, ICadenceType cadenceType)
        {
            switch (cadenceType)
            {
                case CadenceEnumType enumType:
                    enumType.TypeId = (string)jObject.Property("typeID");
                    _compositeDictionary[enumType.TypeId] = enumType;
                    break;
                case CadenceCompositeType compositeType:
                    compositeType.TypeId = (string)jObject.Property("typeID");
                    _compositeDictionary[compositeType.TypeId] = compositeType;
                    break;
            }
        }

        public override ICadenceType Create(Type objectType) => throw new NotImplementedException();
    }
}
