using Flow.Net.Sdk.Crypto;
using Google.Protobuf;
using Nethereum.RLP;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Flow.Net.Sdk
{
    public class FlowTransaction : FlowTransactionBase
    {
        public FlowTransaction()
        {
            SignerList = new Dictionary<ByteString, int>();
        }

        private static IDictionary<ByteString, int> SignerList { get; set; }        

        public static byte[] CanonicalPayload(FlowTransaction flowTransaction)
        {
            var argArray = new List<byte[]>();
            foreach (var argument in flowTransaction.Arguments)
                argArray.Add(RLP.EncodeElement(argument.ToByteArray()));

            var authArray = new List<byte[]>();
            foreach (var authorizer in flowTransaction.Authorizers)
                authArray.Add(RLP.EncodeElement(Utilities.Pad(authorizer.ToByteArray(), 8)));

            var payloadElements = new List<byte[]>
            {
                RLP.EncodeElement(flowTransaction.Script.ToBytesForRLPEncoding()),
                RLP.EncodeList(argArray.ToArray()),
                RLP.EncodeElement(Utilities.Pad(flowTransaction.ReferenceBlockId.ToByteArray(), 32)),
                RLP.EncodeElement(ConvertorForRLPEncodingExtensions.ToBytesFromNumber(BitConverter.GetBytes(flowTransaction.GasLimit))),
                RLP.EncodeElement(Utilities.Pad(flowTransaction.ProposalKey.Address.ToByteArray(), 8)),
                RLP.EncodeElement(ConvertorForRLPEncodingExtensions.ToBytesFromNumber(BitConverter.GetBytes(flowTransaction.ProposalKey.KeyId))),
                RLP.EncodeElement(ConvertorForRLPEncodingExtensions.ToBytesFromNumber(BitConverter.GetBytes(flowTransaction.ProposalKey.SequenceNumber))),
                RLP.EncodeElement(Utilities.Pad(flowTransaction.Payer.ToByteArray(), 8)),
                RLP.EncodeList(authArray.ToArray())
            };

            return RLP.EncodeList(payloadElements.ToArray());
        }

        private static byte[] SignaturesRLP(FlowSignature[] signatures)
        {
            var signatureElements = new List<byte[]>();
            for (var i = 0; i < signatures.Length; i++)
            {
                var index = i;
                if(SignerList.ContainsKey(signatures[i].Address))
                {
                    index = SignerList[signatures[i].Address];
                }
                else
                {
                    SignerList.Add(signatures[i].Address, i);
                }                

                var signatureArray = new List<byte[]>
                {
                    RLP.EncodeElement(index.ToBytesForRLPEncoding()),
                    RLP.EncodeElement(ConvertorForRLPEncodingExtensions.ToBytesFromNumber(BitConverter.GetBytes(signatures[i].KeyId))),
                    RLP.EncodeElement(signatures[i].Signature)
                };
                signatureElements.Add(RLP.EncodeList(signatureArray.ToArray()));
            }

            return RLP.EncodeList(signatureElements.ToArray());
        }

        public static byte[] CanonicalAuthorizationEnvelope(FlowTransaction flowTransaction)
        {
            var authEnvelopeElements = new List<byte[]>
            {
                CanonicalPayload(flowTransaction),
                SignaturesRLP(flowTransaction.PayloadSignatures.ToArray())
            };

            return RLP.EncodeList(authEnvelopeElements.ToArray());
        }

        public static byte[] CanonicalPaymentEnvelope(FlowTransaction flowTransaction)
        {
            var authEnvelopeElements = new List<byte[]>
            {
                CanonicalAuthorizationEnvelope(flowTransaction),
                SignaturesRLP(flowTransaction.EnvelopeSignatures.ToArray())
            };

            return RLP.EncodeList(authEnvelopeElements.ToArray());
        }

        public static byte[] CanonicalTransaction(FlowTransaction flowTransaction)
        {
            var authEnvelopeElements = new List<byte[]>
            {
                CanonicalPayload(flowTransaction),
                SignaturesRLP(flowTransaction.PayloadSignatures.ToArray()),
                SignaturesRLP(flowTransaction.EnvelopeSignatures.ToArray())
            };

            return RLP.EncodeList(authEnvelopeElements.ToArray());
        }
    }

    public static class FlowTransactionExtensions
    {
        public static FlowTransaction AddPayloadSignature(this FlowTransaction flowTransaction, ByteString address, uint keyId, ISigner signer)
        {
            var canonicalPayload = FlowTransaction.CanonicalPayload(flowTransaction);
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
            var canonicalAuthorizationEnvelope = FlowTransaction.CanonicalAuthorizationEnvelope(flowTransaction);
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
