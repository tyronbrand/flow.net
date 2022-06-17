using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace Flow.Net.Sdk.Cadence.Types
{
    public class CadenceRepeatedTypeConverter : JsonConverter
    {
        private Dictionary<string, ICadenceType> _repeatedIds = new Dictionary<string, ICadenceType>();        

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(CadenceCompositeType) || objectType == typeof(CadenceEnumType);
        }

        public override bool CanRead => false;

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if(value is ICadenceType cadenceType)
            {
                string key = "";
                string typeId = "";

                if (value is CadenceEnumType cadenceEnum)
                {
                    key = $"{cadenceEnum.Kind}-{cadenceEnum.TypeId}";
                    typeId = cadenceEnum.TypeId;
                } 
                else if (value is CadenceCompositeType cadenceComposite)
                {
                    key = $"{cadenceComposite.Kind}-{cadenceComposite.TypeId}";
                    typeId = cadenceComposite.TypeId;
                }

                if (!string.IsNullOrEmpty(key) && !string.IsNullOrEmpty(typeId) && IsRepeatedType(key, cadenceType))
                {                    
                    writer.WriteValue(typeId);
                }
                else
                {   
                    JObject jo = new JObject();
                    //TODO: Replace reflection with JObject loop
                    foreach (PropertyInfo prop in value.GetType().GetProperties())
                    {
                        if (prop.CanRead)
                        {
                            object propVal = prop.GetValue(value, null);
                            if (propVal != null)
                            {
                                jo.Add((prop.Name == "TypeId" ? "typeID" : prop.Name.ToLower()), JToken.FromObject(propVal, serializer));
                            }
                        }
                    }
                    jo.WriteTo(writer);

                }
            }       
        }

        private bool IsRepeatedType(string key, ICadenceType cadenceType)
        {
            if(_repeatedIds.ContainsKey(key))
                return true;

            _repeatedIds.Add(key, cadenceType);
            return false;
        }

        //private void AddRemoveSerializedProperties(JObject jObject, MahMan baseContract)
        //{
        //    jObject.AddFirst(....);

        //    foreach (KeyValuePair<string, JToken> propertyJToken in jObject)
        //    {
        //        if (propertyJToken.Value.Type != JTokenType.Object)
        //            continue;

        //        JToken nestedJObject = propertyJToken.Value;
        //        PropertyInfo clrProperty = baseContract.GetType().GetProperty(propertyJToken.Key);
        //        MahMan nestedObjectValue = clrProperty.GetValue(baseContract) as MahMan;
        //        if (nestedObj != null)
        //            AddRemoveSerializedProperties((JObject)nestedJObject, nestedObjectValue);
        //    }
        //}
    }

}
