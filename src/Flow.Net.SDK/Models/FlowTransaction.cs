using Flow.Net.Sdk.Crypto;
using Flow.Net.Sdk.RecursiveLengthPrefix;
using Google.Protobuf;
using System.Collections.Generic;

namespace Flow.Net.Sdk.Models
{
    /// <summary>
    /// A FlowTransaction is a full transaction object containing a payload and signatures.
    /// </summary>
    public class FlowTransaction : FlowTransactionBase
    {
        public FlowTransaction()
        {
            SignerList = new Dictionary<ByteString, int>();
        }

        public Dictionary<ByteString, int> SignerList { get; set; }

        /// <summary>
        /// Signs the full transaction (TransactionDomainTag + payload) with the specified account key.
        /// </summary>
        /// <param name="flowTransaction"></param>
        /// <param name="address"></param>
        /// <param name="keyId"></param>
        /// <param name="signer"></param>
        /// <returns>A <see cref="FlowTransaction"/> with <see cref="FlowSignature"/> appended to <see cref="FlowTransactionBase.PayloadSignatures"/>.</returns>
        public static FlowTransaction AddPayloadSignature(FlowTransaction flowTransaction, FlowAddress address, uint keyId, ISigner signer)
        {
            var canonicalPayload = Rlp.EncodedCanonicalPayload(flowTransaction);
            var message = DomainTag.AddTransactionDomainTag(canonicalPayload);
            var signature = signer.Sign(message);

            flowTransaction.PayloadSignatures.Add(
                new FlowSignature
                {
                    Address = address.Value,
                    KeyId = keyId,
                    Signature = signature
                });

            return flowTransaction;
        }

        /// <summary>
        /// Signs the full transaction (TransactionDomainTag + payload + <see cref="FlowTransactionBase.PayloadSignatures"/>) with the specified account key.
        /// </summary>
        /// <param name="flowTransaction"></param>
        /// <param name="address"></param>
        /// <param name="keyId"></param>
        /// <param name="signer"></param>
        /// <returns>A <see cref="FlowTransaction"/> with <see cref="FlowSignature"/> appended to <see cref="FlowTransactionBase.EnvelopeSignatures"/>.</returns>
        public static FlowTransaction AddEnvelopeSignature(FlowTransaction flowTransaction, FlowAddress address, uint keyId, ISigner signer)
        {
            var canonicalAuthorizationEnvelope = Rlp.EncodedCanonicalAuthorizationEnvelope(flowTransaction);
            var message = DomainTag.AddTransactionDomainTag(canonicalAuthorizationEnvelope);
            var signature = signer.Sign(message);

            flowTransaction.EnvelopeSignatures.Add(
                new FlowSignature
                {
                    Address = address.Value,
                    KeyId = keyId,
                    Signature = signature
                });

            return flowTransaction;
        }
    }    
}
