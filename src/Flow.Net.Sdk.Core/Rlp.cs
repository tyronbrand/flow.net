using Flow.Net.Sdk.Core.Cadence;
using Flow.Net.Sdk.Core.Models;
using Nethereum.RLP;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Flow.Net.Sdk.Core
{
    public static class Rlp
    {
        public static byte[] EncodedAccountKey(FlowAccountKey flowAccountKey)
        {
            var accountElements = new List<byte[]>
            {
                RLP.EncodeElement(flowAccountKey.PublicKey.HexToBytes()),
                RLP.EncodeElement(ConvertorForRLPEncodingExtensions.ToBytesFromNumber(BitConverter.GetBytes((uint)flowAccountKey.SignatureAlgorithm))),
                RLP.EncodeElement(ConvertorForRLPEncodingExtensions.ToBytesFromNumber(BitConverter.GetBytes((uint)flowAccountKey.HashAlgorithm))),
                RLP.EncodeElement(ConvertorForRLPEncodingExtensions.ToBytesFromNumber(BitConverter.GetBytes(flowAccountKey.Weight)))
            };

            return RLP.EncodeList(accountElements.ToArray());
        }

        public static byte[] EncodedCanonicalPayload(FlowTransaction flowTransaction)
        {
            var payloadElements = new List<byte[]>
            {
                RLP.EncodeElement(flowTransaction.Script.ToBytesForRLPEncoding()),
                RLP.EncodeList(flowTransaction.Arguments.Select(argument => RLP.EncodeElement(argument.Encode().ToBytesForRLPEncoding())).ToArray()),
                RLP.EncodeElement(Utilities.Pad(flowTransaction.ReferenceBlockId.HexToBytes(), 32)),
                RLP.EncodeElement(ConvertorForRLPEncodingExtensions.ToBytesFromNumber(BitConverter.GetBytes(flowTransaction.GasLimit))),
                RLP.EncodeElement(Utilities.Pad(flowTransaction.ProposalKey.Address.Address.HexToBytes(), 8)),
                RLP.EncodeElement(ConvertorForRLPEncodingExtensions.ToBytesFromNumber(BitConverter.GetBytes(flowTransaction.ProposalKey.KeyId))),
                RLP.EncodeElement(ConvertorForRLPEncodingExtensions.ToBytesFromNumber(BitConverter.GetBytes(flowTransaction.ProposalKey.SequenceNumber))),
                RLP.EncodeElement(Utilities.Pad(flowTransaction.Payer.Address.HexToBytes(), 8)),
                RLP.EncodeList(flowTransaction.Authorizers.Select(authorizer => RLP.EncodeElement(Utilities.Pad(authorizer.Address.HexToBytes(), 8))).ToArray())
            };

            return RLP.EncodeList(payloadElements.ToArray());
        }

        private static byte[] EncodedSignatures(IReadOnlyList<FlowSignature> payloadSigners, FlowTransaction flowTransaction)
        {
            var signers = new Dictionary<string, int>();
            var singerIndex = 0;

            signers.Add(flowTransaction.ProposalKey.Address.Address, singerIndex);
            singerIndex++;

            if (!signers.ContainsKey(flowTransaction.Payer.Address))
            {
                signers.Add(flowTransaction.Payer.Address, singerIndex);
                singerIndex++;
            }

            foreach (var authorizer in flowTransaction.Authorizers)
            {
                if (!signers.ContainsKey(authorizer.Address))
                {
                    signers.Add(authorizer.Address, singerIndex);
                    singerIndex++;
                }
            }

            var signatureElements = new List<byte[]>();
            foreach (var payloadSigner in payloadSigners)
            {
                if (signers.ContainsKey(payloadSigner.Address.Address))
                {
                    if (!flowTransaction.SignerList.ContainsKey(payloadSigner.Address.Address))
                        flowTransaction.SignerList.Add(payloadSigner.Address.Address, signers[payloadSigner.Address.Address]);

                    var signatureEncoded = EncodedSignature(payloadSigner, signers[payloadSigner.Address.Address]);
                    signatureElements.Add(signatureEncoded);
                }
            }

            return RLP.EncodeList(signatureElements.ToArray());
        }

        private static byte[] EncodedSignature(FlowSignature signature, int index)
        {
            var signatureArray = new List<byte[]>
            {
                RLP.EncodeElement(index.ToBytesForRLPEncoding()),
                RLP.EncodeElement(ConvertorForRLPEncodingExtensions.ToBytesFromNumber(BitConverter.GetBytes(signature.KeyId))),
                RLP.EncodeElement(signature.Signature)
            };

            return RLP.EncodeList(signatureArray.ToArray());
        }

        public static byte[] EncodedCanonicalAuthorizationEnvelope(FlowTransaction flowTransaction)
        {
            var authEnvelopeElements = new List<byte[]>
            {
                EncodedCanonicalPayload(flowTransaction),
                EncodedSignatures(flowTransaction.PayloadSignatures.ToArray(), flowTransaction)
            };

            return RLP.EncodeList(authEnvelopeElements.ToArray());
        }

        public static byte[] EncodedCanonicalPaymentEnvelope(FlowTransaction flowTransaction)
        {
            var authEnvelopeElements = new List<byte[]>
            {
                EncodedCanonicalAuthorizationEnvelope(flowTransaction),
                EncodedSignatures(flowTransaction.EnvelopeSignatures.ToArray(), flowTransaction)
            };

            return RLP.EncodeList(authEnvelopeElements.ToArray());
        }

        public static byte[] EncodedCanonicalTransaction(FlowTransaction flowTransaction)
        {
            var authEnvelopeElements = new List<byte[]>
            {
                EncodedCanonicalPayload(flowTransaction),
                EncodedSignatures(flowTransaction.PayloadSignatures.ToArray(), flowTransaction),
                EncodedSignatures(flowTransaction.EnvelopeSignatures.ToArray(), flowTransaction)
            };

            return RLP.EncodeList(authEnvelopeElements.ToArray());
        }
    }
}
