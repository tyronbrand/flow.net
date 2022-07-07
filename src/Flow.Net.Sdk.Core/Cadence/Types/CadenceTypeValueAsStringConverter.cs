using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Flow.Net.Sdk.Core.Cadence.Types
{
    public class CadenceTypeValueAsStringConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(CadenceTypeValueAsString);
        }

        public override bool CanRead => false;

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {            
            writer.WriteValue(((CadenceTypeValueAsString)value).Value);
        }
    }
}
