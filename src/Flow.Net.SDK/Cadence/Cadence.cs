using Flow.Net.Sdk.Exceptions;
using Google.Protobuf;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace Flow.Net.Sdk.Cadence
{
    public abstract class Cadence : ICadence
    {
        public virtual string Type { get; }

        /// <summary>
        /// Encodes <see cref="ICadence"/>.
        /// </summary>
        /// <param name="cadence"></param>
        /// <returns>A JSON string representation of <see cref="ICadence"/>.</returns>
        public string Encode(ICadence cadence)
        {
            return JsonConvert.SerializeObject(cadence);
        }

        /// <summary>
        /// Casts <see cref="ICadence"/> to <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cadence"></param>
        /// <returns><typeparamref name="T"/>.</returns>
        public T As<T>(ICadence cadence)
            where T : ICadence
        {
            return (T)cadence;
        }

        /// <summary>
        /// Filters <see cref="CadenceCompositeItem.Fields"/> where <see cref="CadenceCompositeItemValue.Name"/> is equal to <paramref name="fieldName"/> and returns the <see cref="CadenceCompositeItemValue.Value"/>.
        /// </summary>
        /// <param name="cadenceComposite"></param>
        /// <param name="fieldName"></param>
        /// <returns>A <see cref="ICadence"/> that satisfies the condition.</returns>
        public ICadence CompositeField(CadenceComposite cadenceComposite, string fieldName)
        {
            ICadence cadenceCompositeValue = cadenceComposite.Value.Fields.Where(w => w.Name == fieldName).Select(s => s.Value).FirstOrDefault();

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
            ICadence cadenceCompositeValue = cadenceComposite.CompositeField(fieldName);

            return cadenceCompositeValue.As<T>();
        }

        /// <summary>
        /// Encodes a <see cref="IEnumerable{T}" /> of <see cref="ICadence"/> to <see cref="IList{T}" /> of <see cref="ByteString"/>.
        /// </summary>
        /// <param name="cadenceValues"></param>
        /// <returns>Arguments JSON encoded as a <see cref="IList{T}" /> of <see cref="ByteString"/>.</returns>
        public IList<ByteString> GenerateTransactionArguments(IEnumerable<ICadence> cadenceValues)
        {
            var arguments = new List<ByteString>();

            if (cadenceValues != null && cadenceValues.Any())
            {
                foreach (var value in cadenceValues)
                    arguments.Add(value.Encode().FromStringToByteString());
            }

            return arguments;
        }
    }
}
