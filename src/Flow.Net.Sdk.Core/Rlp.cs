using Flow.Net.Sdk.Core.Cadence;
using Flow.Net.Sdk.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Flow.Net.Sdk.Core
{
    public class Rlp
    {
        public static byte[] EncodedAccountKey(FlowAccountKey flowAccountKey)
        {
            var accountElements = new List<byte[]>
            {
                EncodeElement(flowAccountKey.PublicKey.HexToBytes()),
                EncodeElement(ToBytesFromNumber(BitConverter.GetBytes((uint)flowAccountKey.SignatureAlgorithm))),
                EncodeElement(ToBytesFromNumber(BitConverter.GetBytes((uint)flowAccountKey.HashAlgorithm))),
                EncodeElement(ToBytesFromNumber(BitConverter.GetBytes(flowAccountKey.Weight)))
            };

            return EncodeList(accountElements.ToArray());
        }

        public static byte[] EncodedCanonicalPayload(FlowTransaction flowTransaction)
        {
            var payloadElements = new List<byte[]>
            {
                EncodeElement(Encoding.UTF8.GetBytes(flowTransaction.Script)),
                EncodeList(flowTransaction.Arguments.Select(argument => EncodeElement(Encoding.UTF8.GetBytes(argument.Encode()))).ToArray()),
                EncodeElement(Utilities.Pad(flowTransaction.ReferenceBlockId.HexToBytes(), 32)),
                EncodeElement(ToBytesFromNumber(BitConverter.GetBytes(flowTransaction.GasLimit))),
                EncodeElement(Utilities.Pad(flowTransaction.ProposalKey.Address.Address.HexToBytes(), 8)),
                EncodeElement(ToBytesFromNumber(BitConverter.GetBytes(flowTransaction.ProposalKey.KeyId))),
                EncodeElement(ToBytesFromNumber(BitConverter.GetBytes(flowTransaction.ProposalKey.SequenceNumber))),
                EncodeElement(Utilities.Pad(flowTransaction.Payer.Address.HexToBytes(), 8)),
                EncodeList(flowTransaction.Authorizers.Select(authorizer => EncodeElement(Utilities.Pad(authorizer.Address.HexToBytes(), 8))).ToArray())
            };

            return EncodeList(payloadElements.ToArray());
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

            return EncodeList(signatureElements.ToArray());
        }

        private static byte[] EncodedSignature(FlowSignature signature, int index)
        {
            var signatureArray = new List<byte[]>
            {
                EncodeElement(ToBytesFromNumber(BitConverter.GetBytes(index))),
                EncodeElement(ToBytesFromNumber(BitConverter.GetBytes(signature.KeyId))),
                EncodeElement(signature.Signature)
            };

            return EncodeList(signatureArray.ToArray());
        }

        public static byte[] EncodedCanonicalAuthorizationEnvelope(FlowTransaction flowTransaction)
        {
            var authEnvelopeElements = new List<byte[]>
            {
                EncodedCanonicalPayload(flowTransaction),
                EncodedSignatures(flowTransaction.PayloadSignatures.ToArray(), flowTransaction)
            };

            return EncodeList(authEnvelopeElements.ToArray());
        }

        public static byte[] EncodedCanonicalPaymentEnvelope(FlowTransaction flowTransaction)
        {
            var authEnvelopeElements = new List<byte[]>
            {
                EncodedCanonicalAuthorizationEnvelope(flowTransaction),
                EncodedSignatures(flowTransaction.EnvelopeSignatures.ToArray(), flowTransaction)
            };

            return EncodeList(authEnvelopeElements.ToArray());
        }

        public static byte[] EncodedCanonicalTransaction(FlowTransaction flowTransaction)
        {
            var authEnvelopeElements = new List<byte[]>
            {
                EncodedCanonicalPayload(flowTransaction),
                EncodedSignatures(flowTransaction.PayloadSignatures.ToArray(), flowTransaction),
                EncodedSignatures(flowTransaction.EnvelopeSignatures.ToArray(), flowTransaction)
            };

            return EncodeList(authEnvelopeElements.ToArray());
        }

        public static byte[] EncodeElement(byte[] srcData)
        {
            if (IsNullOrZeroArray(srcData))
            {
                return new byte[1] { 128 };
            }

            if (IsSingleZero(srcData))
            {
                return srcData;
            }

            if (srcData.Length == 1 && srcData[0] < 128)
            {
                return srcData;
            }

            if (srcData.Length < 56)
            {
                byte b = (byte)(128 + srcData.Length);
                byte[] array = new byte[srcData.Length + 1];
                Array.Copy(srcData, 0, array, 1, srcData.Length);
                array[0] = b;
                return array;
            }

            int num = srcData.Length;
            byte b2 = 0;
            while (num != 0)
            {
                b2 = (byte)(b2 + 1);
                num >>= 8;
            }

            byte[] array2 = new byte[b2];
            for (int i = 0; i < b2; i++)
            {
                array2[b2 - 1 - i] = (byte)(srcData.Length >> 8 * i);
            }

            byte[] array3 = new byte[srcData.Length + 1 + b2];
            Array.Copy(srcData, 0, array3, 1 + b2, srcData.Length);
            array3[0] = (byte)(183 + b2);
            Array.Copy(array2, 0, array3, 1, array2.Length);
            return array3;
        }

        public static byte[] EncodeList(params byte[][] items)
        {
            if (items == null || (items.Length == 1 && items[0] == null))
            {
                return new byte[1] { 192 };
            }

            int num = 0;
            for (int i = 0; i < items.Length; i++)
            {
                num += items[i].Length;
            }

            byte[] array;
            int num2;
            if (num < 56)
            {
                array = new byte[1 + num];
                array[0] = (byte)(192 + num);
                num2 = 1;
            }
            else
            {
                int num3 = num;
                byte b = 0;
                while (num3 != 0)
                {
                    b = (byte)(b + 1);
                    num3 >>= 8;
                }

                num3 = num;
                byte[] array2 = new byte[b];
                for (int j = 0; j < b; j++)
                {
                    array2[b - 1 - j] = (byte)(num3 >> 8 * j);
                }

                array = new byte[1 + array2.Length + num];
                array[0] = (byte)(247 + b);
                Array.Copy(array2, 0, array, 1, array2.Length);
                num2 = array2.Length + 1;
            }

            foreach (byte[] array3 in items)
            {
                Array.Copy(array3, 0, array, num2, array3.Length);
                num2 += array3.Length;
            }

            return array;
        }

        public static bool IsNullOrZeroArray(byte[] array)
        {
            if (array != null)
            {
                return array.Length == 0;
            }

            return true;
        }

        public static bool IsSingleZero(byte[] array)
        {
            if (array.Length == 1)
            {
                return array[0] == 0;
            }

            return false;
        }

        public static byte[] ToBytesFromNumber(byte[] bytes)
        {
            if (BitConverter.IsLittleEndian)
                bytes = bytes.Reverse().ToArray();

            return TrimZeroBytes(bytes);
        }

        public static byte[] TrimZeroBytes(byte[] bytes)
        {
            var trimmed = new List<byte>();
            var previousByteWasZero = true;

            for (var i = 0; i < bytes.Length; i++)
            {
                if (previousByteWasZero && bytes[i] == 0)
                    continue;

                previousByteWasZero = false;
                trimmed.Add(bytes[i]);
            }

            return trimmed.ToArray();
        }
    }
}
