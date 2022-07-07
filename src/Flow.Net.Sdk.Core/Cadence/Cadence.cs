using Flow.Net.Sdk.Core.Cadence.Types;
using Flow.Net.Sdk.Core.Exceptions;
using Newtonsoft.Json;
using System.Linq;

namespace Flow.Net.Sdk.Core.Cadence
{
    public abstract class Cadence : ICadence
    {
        public virtual string Type { get; set; }

        /// <summary>
        /// Encodes <see cref="ICadence"/>.
        /// </summary>
        /// <param name="cadence"></param>
        /// <returns>A JSON string representation of <see cref="ICadence"/>.</returns>
        public string Encode(ICadence cadence)
        {
            JsonConverter[] jsonConverters = { new CadenceRepeatedTypeConverter(), new CadenceTypeValueAsStringConverter() };
            return JsonConvert.SerializeObject(cadence, jsonConverters);
        }

        /// <summary>
        /// Filters <see cref="CadenceCompositeItem.Fields"/> where <see cref="CadenceCompositeItemValue.Name"/> is equal to <paramref name="fieldName"/> and returns the <see cref="CadenceCompositeItemValue.Value"/>.
        /// </summary>
        /// <param name="cadenceComposite"></param>
        /// <param name="fieldName"></param>
        /// <returns>A <see cref="ICadence"/> that satisfies the condition.</returns>
        public ICadence CompositeField(CadenceComposite cadenceComposite, string fieldName)
        {
            var cadenceCompositeValue = cadenceComposite.Value.Fields.Where(w => w.Name == fieldName).Select(s => s.Value).FirstOrDefault();

            return cadenceCompositeValue ?? throw new FlowException($"Failed to find fieldName: {fieldName}");
        }

        /// <summary>
        /// Filters <see cref="CadenceCompositeItem.Fields"/> where <see cref="CadenceCompositeItemValue.Name"/> is equal to <paramref name="fieldName"/> and returns the <see cref="CadenceCompositeItemValue.Value"/> as <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cadenceComposite"></param>
        /// <param name="fieldName"></param>
        /// <returns>A <typeparamref name="T"/> that satisfies the condition.</returns>
        public T CompositeFieldAs<T>(CadenceComposite cadenceComposite, string fieldName)
            where T : ICadence
        {
            return cadenceComposite.CompositeField(fieldName).As<T>();
        }
    }
}
