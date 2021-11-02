using Flow.Net.Sdk.Exceptions;
using Flow.Net.Sdk.Models;
using Google.Protobuf;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace Flow.Net.Sdk.Cadence
{
    public static class CadenceExtensions
    {
        public static string Encode(this ICadence cadence)
        {
            return JsonConvert.SerializeObject(cadence);
        }

        public static ICadence Decode(this string cadenceJson, CadenceConverter cadenceConverter = null)
        {
            return JsonConvert.DeserializeObject<ICadence>(cadenceJson, cadenceConverter ?? new CadenceConverter());
        }

        public static T As<T>(this ICadence cadence) where T : ICadence
        {
            return (T)cadence;
        }

        public static IList<ByteString> ToTransactionArguments(this IEnumerable<ICadence> cadenceValues)
        {
            var arguments = new List<ByteString>();

            if (cadenceValues != null && cadenceValues.Count() > 0)
            {
                foreach (var value in cadenceValues)
                {
                    var serialized = value.Encode();
                    arguments.Add(serialized.FromStringToByteString());
                }
            }

            return arguments;
        }

        public static string AccountCreatedAddress(this IList<FlowEvent> flowEvents)
        {
            return flowEvents.Where(w => w.Type == "flow.AccountCreated")
                    .FirstOrDefault().Payload.As<CadenceComposite>()
                    .Value.Fields.FirstOrDefault().Value.As<CadenceAddress>()
                    .Value.Remove0x();
        }

        public static ICadence CompositeField(this CadenceComposite cadenceComposite, string fieldName)
        {
            return cadenceComposite.Value.Fields.Where(w => w.Name == fieldName).Select(s => s.Value).FirstOrDefault();
        }

        public static T CompositeFieldAs<T>(this CadenceComposite cadenceComposite, string fieldName) where T : ICadence
        {
            var cadenceCompositeValue = cadenceComposite.CompositeField(fieldName);

            if (cadenceCompositeValue == null)
                throw new FlowException($"Failed to find fieldName: {fieldName}");

            return cadenceCompositeValue.As<T>();
        }
    }
}
