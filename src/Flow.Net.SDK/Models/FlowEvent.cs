using Flow.Net.Sdk.Cadence;
using Flow.Net.Sdk.Constants;
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
                return null;

            if (accountCreatedEvent.Payload == null)
                return null;

            var compositeItemFields = accountCreatedEvent.Payload.As<CadenceComposite>().Value.Fields.ToList();

            if (!compositeItemFields.Any())
                return null;

            var addressValue = compositeItemFields.FirstOrDefault();

            if (addressValue == null)
                return null;

            return new FlowAddress(addressValue.Value.As<CadenceAddress>().Value);
        }
    }
}
