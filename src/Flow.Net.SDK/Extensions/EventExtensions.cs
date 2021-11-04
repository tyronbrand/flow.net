using Google.Protobuf;
using System.Collections.Generic;

namespace Flow.Net.Sdk.Models
{
    public static class EventExtensions
    {
        ///<inheritdoc cref="FlowEvent.AccountCreatedAddress"/>
        public static ByteString AccountCreatedAddress(this IList<FlowEvent> flowEvents)
        {
            return FlowEvent.AccountCreatedAddress(flowEvents);
        }
    }
}
