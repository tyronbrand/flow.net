using Flow.Net.Sdk.Crypto;
using Nethereum.RLP;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Flow.Net.Sdk.Models
{
    public class FlowAccountKey
    {
        public uint Index { get; set; }
        public string PublicKey { get; set; }
        public string PrivateKey { get; set; }
        public SignatureAlgo SignatureAlgorithm { get; set; }
        public HashAlgo HasAlgorithm { get; set; }
        public uint Weight { get; set; }
        public ulong SequenceNumber { get; set; }
        public bool Revoked { get; set; }
        public ISigner Signer { get; set; }

        public static FlowAccountKey NewEcdsaAccountKey(SignatureAlgo signatureAlgo, HashAlgo hashAlgo, uint weight)
        {
            var newKeys = Crypto.Ecdsa.Utilities.GenerateKeyPair(signatureAlgo);
            var publicKey = Crypto.Ecdsa.Utilities.DecodePublicKeyToHex(newKeys);
            var privateKey = Crypto.Ecdsa.Utilities.DecodePrivateKeyToHex(newKeys);

            return new FlowAccountKey
            {
                PrivateKey = privateKey,
                PublicKey = publicKey,
                SignatureAlgorithm = signatureAlgo,
                HasAlgorithm = hashAlgo,                
                Weight = weight
            };
        }

        public static byte[] RlpEncode(FlowAccountKey flowAccountKey)
        {
            var accountElements = new List<byte[]>
            {
                RLP.EncodeElement(flowAccountKey.PublicKey.FromHexToBytes()),
                RLP.EncodeElement(ConvertorForRLPEncodingExtensions.ToBytesFromNumber(BitConverter.GetBytes((uint)flowAccountKey.SignatureAlgorithm))),
                RLP.EncodeElement(ConvertorForRLPEncodingExtensions.ToBytesFromNumber(BitConverter.GetBytes((uint)flowAccountKey.HasAlgorithm))),
                RLP.EncodeElement(ConvertorForRLPEncodingExtensions.ToBytesFromNumber(BitConverter.GetBytes(flowAccountKey.Weight)))
            };

            return RLP.EncodeList(accountElements.ToArray());
        }

        public static IList<FlowAccountKey> UpdateFlowAccountKeys(IList<FlowAccountKey> currentFlowAccountKeys, IList<FlowAccountKey> updatedFlowAccountKeys)
        {
            foreach(var key in updatedFlowAccountKeys)
            {
                var currentKey = currentFlowAccountKeys.Where(w => w.PublicKey == key.PublicKey).FirstOrDefault();
                if(currentKey != null && !string.IsNullOrEmpty(currentKey.PrivateKey))
                {
                    key.PrivateKey = currentKey.PrivateKey;
                    key.Signer = Crypto.Ecdsa.Utilities.CreateSigner(key.PrivateKey, key.SignatureAlgorithm, key.HasAlgorithm);
                }
            }

            return updatedFlowAccountKeys;
        }
    }
}
