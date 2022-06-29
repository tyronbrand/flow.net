using System;
using System.Runtime.Serialization;

namespace Flow.Net.Sdk.Core.Exceptions
{
    [Serializable]
    public class FlowException : Exception
    {
        public FlowException(string message) : base(message) { }
        public FlowException(string message, Exception inner) : base(message, inner) { }

        protected FlowException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
