using Flow.Net.Sdk.Crypto;
using Flow.Net.Sdk.RecursiveLengthPrefix;
using Google.Protobuf;

namespace Flow.Net.Sdk.Models
{
    public static class FlowTransactionExtensions
    {
        public static FlowTransaction AddPayloadSignature(this FlowTransaction flowTransaction, ByteString address, uint keyId, ISigner signer)
        {
            var canonicalPayload = Rlp.EncodedCanonicalPayload(flowTransaction);
            var message = DomainTag.AddTransactionDomainTag(canonicalPayload);
            var signature = signer.Sign(message);

            flowTransaction.PayloadSignatures.Add(
                new FlowSignature
                {
                    Address = address,
                    KeyId = keyId,
                    Signature = signature
                });

            return flowTransaction;
        }

        public static FlowTransaction AddEnvelopeSignature(this FlowTransaction flowTransaction, ByteString address, uint keyId, ISigner signer)
        {
            var canonicalAuthorizationEnvelope = Rlp.EncodedCanonicalAuthorizationEnvelope(flowTransaction);
            var message = DomainTag.AddTransactionDomainTag(canonicalAuthorizationEnvelope);
            var signature = signer.Sign(message);

            flowTransaction.EnvelopeSignatures.Add(
                new FlowSignature
                {
                    Address = address,
                    KeyId = keyId,
                    Signature = signature
                });

            return flowTransaction;
        }
    }
}
