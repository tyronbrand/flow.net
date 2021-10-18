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

        public static T AsCadenceType<T>(this ICadence cadence) where T : ICadence
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
    }
}
