using Flow.Net.Sdk.RecursiveLengthPrefix;
using Google.Protobuf;
using System.Collections.Generic;

namespace Flow.Net.Sdk.Models
{
    public class FlowTransaction : FlowTransactionBase
    {
        public FlowTransaction()
        {
            SignerList = new Dictionary<ByteString, int>();
        }

        private static Dictionary<ByteString, int> SignerList { get; set; }

        public static byte[] CanonicalPayload(FlowTransaction flowTransaction)
        {
            return Rlp.EncodedCanonicalPayload(flowTransaction);
        }

        public static byte[] CanonicalAuthorizationEnvelope(FlowTransaction flowTransaction)
        {
            return Rlp.EncodedCanonicalAuthorizationEnvelope(flowTransaction, SignerList);
        }

        public static byte[] CanonicalPaymentEnvelope(FlowTransaction flowTransaction)
        {
            return Rlp.EncodedCanonicalPaymentEnvelope(flowTransaction, SignerList);
        }

        public static byte[] CanonicalTransaction(FlowTransaction flowTransaction)
        {
            return Rlp.EncodedCanonicalTransaction(flowTransaction, SignerList);
        }
    }    
}
