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
        /// Filters a <see cref="IList{T}" /> of type <see cref="FlowEvent"/> where <see cref="Type"/> is equal to "flow.AccountCreated" and returns the account address.
        /// </summary>
        /// <param name="flowEvents"></param>
        /// <returns>An account address as a <see cref="ByteString"/>.</returns>
        public static ByteString AccountCreatedAddress(IList<FlowEvent> flowEvents)
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
    }
}
