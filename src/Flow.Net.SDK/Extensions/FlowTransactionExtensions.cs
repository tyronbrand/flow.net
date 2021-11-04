using Flow.Net.Sdk.Crypto;
using Google.Protobuf;

namespace Flow.Net.Sdk.Models
{
    public static class FlowTransactionExtensions
    {
        ///<inheritdoc cref="FlowTransaction.AddPayloadSignature"/>
        public static FlowTransaction AddPayloadSignature(this FlowTransaction flowTransaction, ByteString address, uint keyId, ISigner signer)
        {
            return FlowTransaction.AddPayloadSignature(flowTransaction, address, keyId, signer);
        }

        ///<inheritdoc cref="FlowTransaction.AddEnvelopeSignature"/>
        public static FlowTransaction AddEnvelopeSignature(this FlowTransaction flowTransaction, ByteString address, uint keyId, ISigner signer)
        {
            return FlowTransaction.AddEnvelopeSignature(flowTransaction, address, keyId, signer);
        }
    }
}
