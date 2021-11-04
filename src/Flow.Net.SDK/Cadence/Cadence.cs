﻿using Flow.Net.Sdk.Constants;
using Flow.Net.Sdk.Exceptions;
using Flow.Net.Sdk.Models;
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
        /// Decodes a <see cref="ICadence"/> JSON string.
        /// </summary>
        /// <param name="cadenceJson"></param>
        /// <param name="cadenceConverter"></param>
        /// <returns>The deserialized <see cref="ICadence"/> from the JSON string.</returns>
        public ICadence Decode(string cadenceJson, CadenceConverter cadenceConverter = null)
        {
            return JsonConvert.DeserializeObject<ICadence>(cadenceJson, cadenceConverter ?? new CadenceConverter());
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
        /// Filters a <see cref="IList{T}" /> of type <see cref="FlowEvent"/> where <see cref="Type"/> is equal to "flow.AccountCreated" and returns the account address.
        /// </summary>
        /// <param name="flowEvents"></param>
        /// <returns>An account address as a <see cref="ByteString"/>.</returns
        public ByteString AccountCreatedAddress(IList<FlowEvent> flowEvents)
        {
            var accountCreatedEvent = flowEvents.FirstOrDefault(w => w.Type == Event.AccountCreated);

            if (accountCreatedEvent == null)
                throw new FlowException($"Failed to find event with type \"{Event.AccountCreated}\".");

            if (accountCreatedEvent.Payload == null)
                throw new FlowException($"Payload for event type \"{Event.AccountCreated}\" can not be null.");

            var compItemFields = accountCreatedEvent.Payload.As<CadenceComposite>().Value.Fields;

            if (compItemFields.Count() == 0)
                throw new FlowException($"Payload fields can not be empty.");

            return compItemFields.FirstOrDefault()
                .Value.As<CadenceAddress>()
                .Value.FromHexToByteString();
        }

        /// <summary>
        /// Filters <see cref="CadenceCompositeItem.Fields"/> where <see cref="CadenceCompositeItemValue.Name"/> is equal to <paramref name="fieldName"/> and returns the <see cref="CadenceCompositeItemValue.Value"/>.
        /// </summary>
        /// <param name="cadenceComposite"></param>
        /// <param name="fieldName"></param>
        /// <returns>A <see cref="ICadence"/> that satisfies the condition.</returns>
        public ICadence CompositeField(CadenceComposite cadenceComposite, string fieldName)
        {
            return cadenceComposite.Value.Fields.Where(w => w.Name == fieldName).Select(s => s.Value).FirstOrDefault();
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
            var cadenceCompositeValue = cadenceComposite.CompositeField(fieldName);

            if (cadenceCompositeValue == null)
                throw new FlowException($"Failed to find fieldName: {fieldName}");

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

            if (cadenceValues != null && cadenceValues.Count() > 0)
            {
                foreach (var value in cadenceValues)
                    arguments.Add(value.Encode().FromStringToByteString());
            }

            return arguments;
        }
    }
}