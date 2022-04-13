using Flow.Net.Sdk.Crypto;
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

        public Dictionary<ByteString, int> SignerList { get; }
        public IList<FlowSigner> PayloadSigners { get; set; } = new List<FlowSigner>();
        public IList<FlowSigner> EnvelopeSigners { get; set; } = new List<FlowSigner>();

        /// <summary>
        /// Signs the full transaction (TransactionDomainTag + payload) with the specified account key.
        /// </summary>
        /// <param name="flowTransaction"></param>
        /// <param name="address"></param>
        /// <param name="keyId"></param>
        /// <param name="signer"></param>
        /// <returns>A <see cref="FlowTransaction"/> with <see cref="FlowSignature"/> appended to <see cref="FlowTransactionBase.PayloadSignatures"/>.</returns>
        public static FlowTransaction AddPayloadSigner(FlowTransaction flowTransaction, FlowAddress address, uint keyId, ISigner signer)
        {
            flowTransaction.PayloadSigners.Add(
                new FlowSigner
                {
                    Address = address.Value,
                    KeyId = keyId,
                    Signer = signer
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
        public static FlowTransaction AddEnvelopeSigner(FlowTransaction flowTransaction, FlowAddress address, uint keyId, ISigner signer)
        {
            flowTransaction.EnvelopeSigners.Add(
                new FlowSigner
                {
                    Address = address.Value,
                    KeyId = keyId,
                    Signer = signer
                });

            return flowTransaction;
        }
    }    
}
