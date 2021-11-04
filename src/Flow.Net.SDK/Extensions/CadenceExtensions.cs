using Flow.Net.Sdk.Exceptions;
using Google.Protobuf;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace Flow.Net.Sdk.Cadence
{
    public static class CadenceExtensions
    {
        ///<inheritdoc cref="Cadence.Encode"/>
        public static string Encode(this ICadence cadence)
        {
            return cadence.Encode(cadence);
        }

        ///<inheritdoc cref="Cadence.Decode"/>
        public static ICadence Decode(this string cadenceJson, CadenceConverter cadenceConverter = null)
        {
            return JsonConvert.DeserializeObject<ICadence>(cadenceJson, cadenceConverter ?? new CadenceConverter());
        }

        ///<inheritdoc cref="Cadence.As"/>
        public static T As<T>(this ICadence cadence)
            where T : ICadence
        {
            return cadence.As<T>(cadence);
        }

        ///<inheritdoc cref="Cadence.CompositeField"/>
        public static ICadence CompositeField(this CadenceComposite cadenceComposite, string fieldName)
        {
            return cadenceComposite.Value.Fields.Where(w => w.Name == fieldName).Select(s => s.Value).FirstOrDefault();
        }

        ///<inheritdoc cref="Cadence.CompositeFieldAs"/>
        public static T CompositeFieldAs<T>(this CadenceComposite cadenceComposite, string fieldName)
            where T : ICadence
        {
            var cadenceCompositeValue = cadenceComposite.CompositeField(fieldName);

            if (cadenceCompositeValue == null)
                throw new FlowException($"Failed to find fieldName: {fieldName}");

            return cadenceCompositeValue.As<T>();
        }

        ///<inheritdoc cref="Cadence.GenerateTransactionArguments"/>
        public static IList<ByteString> GenerateTransactionArguments(this IEnumerable<ICadence> cadenceValues)
        {
            var arguments = new List<ByteString>();

            if (cadenceValues != null && cadenceValues.Count() > 0)
            {
                foreach (var value in cadenceValues)
                    arguments.Add(value.Encode().FromStringToByteString());
            }

            return arguments;
        }
    }
}
