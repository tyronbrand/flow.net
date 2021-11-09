using Newtonsoft.Json;

namespace Flow.Net.Sdk.Cadence
{
    public static class CadenceExtensions
    {
        ///<inheritdoc cref="Cadence.Encode"/>
        public static string Encode(this ICadence cadence)
        {
            return cadence.Encode(cadence);
        }

        /// <summary>
        /// Decodes a <see cref="ICadence"/> JSON string.
        /// </summary>
        /// <param name="cadenceJson"></param>
        /// <param name="cadenceConverter"></param>
        /// <returns>The deserialized <see cref="ICadence"/> from the JSON string.</returns>
        public static ICadence Decode(this string cadenceJson, CadenceConverter cadenceConverter = null)
        {
            return JsonConvert.DeserializeObject<ICadence>(cadenceJson, cadenceConverter ?? new CadenceConverter());
        }

        /// <summary>
        /// Casts <see cref="ICadence"/> to <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cadence"></param>
        /// <returns><typeparamref name="T"/>.</returns>
        public static T As<T>(this ICadence cadence)
            where T : ICadence
        {
            return (T)cadence;
        }

        ///<inheritdoc cref="Cadence.CompositeField"/>
        public static ICadence CompositeField(this CadenceComposite cadenceComposite, string fieldName)
        {
            return cadenceComposite.CompositeField(cadenceComposite, fieldName);
        }

        ///<inheritdoc cref="Cadence.CompositeFieldAs{T}(CadenceComposite, string)"/>
        public static T CompositeFieldAs<T>(this CadenceComposite cadenceComposite, string fieldName)
            where T : ICadence
        {
            return cadenceComposite.CompositeFieldAs<T>(cadenceComposite, fieldName);
        }
    }
}
