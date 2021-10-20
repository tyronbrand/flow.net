using System;

namespace Flow.Net.Sdk.Exceptions
{
    public class FlowException : Exception
    {
        public FlowException(string message, Exception exception = null)
           : base(message, exception)
        {
        }
    }
}
