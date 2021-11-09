using Flow.Net.Sdk.Cadence;
using Flow.Net.Sdk.Constants;
using Flow.Net.Sdk.Exceptions;
using Google.Protobuf;
using System.Collections.Generic;
using System.Linq;

namespace Flow.Net.Sdk.Models
{
    public class FlowEvent
    {        
        public uint EventIndex { get; set; }
        public ICadence Payload { get; set; }
        public ByteString TransactionId { get; set; }
        public uint TransactionIndex { get; set; }
        public string Type { get; set; }

        /// <summary>
        /// Filters a <see cref="IEnumerable{T}" /> of type <see cref="FlowEvent"/> where <see cref="Type"/> is equal to "flow.AccountCreated" and returns a <see cref="FlowAddress"/>.
        /// </summary>
        /// <param name="flowEvents"></param>
        /// <returns>A <see cref="FlowAddress"/> that satisfies the condition.</returns>
        public static FlowAddress AccountCreatedAddress(IEnumerable<FlowEvent> flowEvents)
        {
            var accountCreatedEvent = flowEvents.FirstOrDefault(w => w.Type == Event.AccountCreated);

            if (accountCreatedEvent == null)
                throw new FlowException($"Failed to find event with type \"{Event.AccountCreated}\".");

            if (accountCreatedEvent.Payload == null)
                throw new FlowException($"Payload for event type \"{Event.AccountCreated}\" can not be null.");

            var compositeItemFields = accountCreatedEvent.Payload.As<CadenceComposite>().Value.Fields.ToList();

            if (!compositeItemFields.Any())
                throw new FlowException("Payload fields can not be empty.");

            var addressValue = compositeItemFields.FirstOrDefault();

            if (addressValue == null)
                throw new FlowException("Address can not be null.");

            return new FlowAddress(addressValue.Value.As<CadenceAddress>().Value);
        }
    }
}
