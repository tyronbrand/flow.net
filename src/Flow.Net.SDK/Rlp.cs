using Flow.Net.Sdk.Cadence;
using Flow.Net.Sdk.Models;
using Nethereum.RLP;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Flow.Net.Sdk
{
    public static class Rlp
    {
        public static byte[] EncodedAccountKey(FlowAccountKey flowAccountKey)
        {
            var accountElements = new List<byte[]>
            {
                RLP.EncodeElement(flowAccountKey.PublicKey.FromHexToBytes()),
                RLP.EncodeElement(ConvertorForRLPEncodingExtensions.ToBytesFromNumber(BitConverter.GetBytes((uint)flowAccountKey.SignatureAlgorithm))),
                RLP.EncodeElement(ConvertorForRLPEncodingExtensions.ToBytesFromNumber(BitConverter.GetBytes((uint)flowAccountKey.HashAlgorithm))),
                RLP.EncodeElement(ConvertorForRLPEncodingExtensions.ToBytesFromNumber(BitConverter.GetBytes(flowAccountKey.Weight)))
            };

            return RLP.EncodeList(accountElements.ToArray());
        }

        public static void AddTransactionSignatures(Protos.entities.Transaction tx, FlowTransaction flowTransaction)
        {
            var canonicalPayload = EncodedCanonicalPayload(tx);
            var payloadSignatures = flowTransaction.PayloadSigners
                .Select(x => x.SignatureFromSigner(canonicalPayload));

            tx.PayloadSignatures.AddRange(payloadSignatures.Select(x => x.FromFlowSignature()));

            var canonicalEnvelope = EncodedCanonicalAuthorizationEnvelope(canonicalPayload, payloadSignatures);
            var envelopeSignatures = flowTransaction.EnvelopeSigners
                .Select(x => x.SignatureFromSigner(canonicalEnvelope));

            tx.EnvelopeSignatures.AddRange(envelopeSignatures.Select(x => x.FromFlowSignature()));
        }

        private static FlowSignature SignatureFromSigner(this FlowSigner signer, byte[] canonicalPayload)
        {
            var message = DomainTag.AddTransactionDomainTag(canonicalPayload);
            var signature = signer.Signer.Sign(message);

            return new FlowSignature
            {
                Address = signer.Address,
                KeyId = signer.KeyId,
                Signature = signature
            };
        }

        private static byte[] EncodedCanonicalPayload(Protos.entities.Transaction tx)
        {
            var payloadElements = new List<byte[]>
            {
                RLP.EncodeElement(tx.Script.FromByteStringToString().ToBytesForRLPEncoding()),
                RLP.EncodeList(tx.Arguments.Select(argument => RLP.EncodeElement(argument.FromByteStringToString().ToBytesForRLPEncoding())).ToArray()),
                RLP.EncodeElement(Utilities.Pad(tx.ReferenceBlockId.ToByteArray(), 32)),
                RLP.EncodeElement(ConvertorForRLPEncodingExtensions.ToBytesFromNumber(BitConverter.GetBytes(tx.GasLimit))),
                RLP.EncodeElement(Utilities.Pad(tx.ProposalKey.Address.ToByteArray(), 8)),
                RLP.EncodeElement(ConvertorForRLPEncodingExtensions.ToBytesFromNumber(BitConverter.GetBytes(tx.ProposalKey.KeyId))),
                RLP.EncodeElement(ConvertorForRLPEncodingExtensions.ToBytesFromNumber(BitConverter.GetBytes(tx.ProposalKey.SequenceNumber))),
                RLP.EncodeElement(Utilities.Pad(tx.Payer.ToByteArray(), 8)),
                RLP.EncodeList(tx.Authorizers.Select(authorizer => RLP.EncodeElement(Utilities.Pad(authorizer.ToByteArray(), 8))).ToArray())
            };

            return RLP.EncodeList(payloadElements.ToArray());
        }

        private static byte[] EncodedSignatures(IReadOnlyList<FlowSignature> signatures, FlowTransaction flowTransaction)
        {
            var signatureElements = new List<byte[]>();
            for (var i = 0; i < signatures.Count; i++)
            {
                var index = i;
                if (flowTransaction.SignerList.ContainsKey(signatures[i].Address))
                {
                    index = flowTransaction.SignerList[signatures[i].Address];
                }
                else
                {
                    flowTransaction.SignerList.Add(signatures[i].Address, i);
                }

                var signatureEncoded = EncodedSignature(signatures[i], index);
                signatureElements.Add(signatureEncoded);
            }

            return RLP.EncodeList(signatureElements.ToArray());
        }

        private static byte[] EncodedSignatures(IReadOnlyList<FlowSignature> signatures)
        {
            var signatureElements = new List<byte[]>();
            var signerList = new Dictionary<Google.Protobuf.ByteString, int>();
            for (var i = 0; i < signatures.Count; i++)
            {
                var index = i;
                if (signerList.ContainsKey(signatures[i].Address))
                {
                    index = signerList[signatures[i].Address];
                }
                else
                {
                    signerList.Add(signatures[i].Address, i);
                }

                var signatureEncoded = EncodedSignature(signatures[i], index);
                signatureElements.Add(signatureEncoded);
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

        private static byte[] EncodedCanonicalAuthorizationEnvelope(byte[] canonicalPayload, IEnumerable<FlowSignature> payloadSignatures)
        {
            var authEnvelopeElements = new List<byte[]>
            {
                canonicalPayload,
                EncodedSignatures(payloadSignatures.ToArray())
            };

            return RLP.EncodeList(authEnvelopeElements.ToArray());
        }

        private static Protos.entities.Transaction.Types.Signature FromFlowSignature(this FlowSignature flowSignature)
        {
            return new Protos.entities.Transaction.Types.Signature
            {
                Address = flowSignature.Address,
                KeyId = flowSignature.KeyId,
                Signature_ = flowSignature.Signature.FromByteArrayToByteString()
            };
        }
    }
}
