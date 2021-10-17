using Newtonsoft.Json;

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
    }
}
