using System.Collections.Generic;

namespace Flow.Net.Sdk.Models
{
    public static class EventExtensions
    {
        ///<inheritdoc cref="FlowEvent.AccountCreatedAddress"/>
        public static FlowAddress AccountCreatedAddress(this IList<FlowEvent> flowEvents)
        {
            return FlowEvent.AccountCreatedAddress(flowEvents);
        }
    }
}
