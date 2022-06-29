using Flow.Net.Sdk.Core.Crypto;
using System.Collections.Generic;
using System.Linq;

namespace Flow.Net.Sdk.Core.Models
{
    /// <summary>
    /// A FlowAccountKey is a public and/or private key associated with an account.
    /// </summary>
    public class FlowAccountKey
    {
        public uint Index { get; set; }
        public string PublicKey { get; set; }
        public string PrivateKey { get; set; }
        public SignatureAlgo SignatureAlgorithm { get; set; }
        public HashAlgo HashAlgorithm { get; set; }
        public uint Weight { get; set; }
        public ulong SequenceNumber { get; set; }
        public bool Revoked { get; set; }
        public ISigner Signer { get; set; }

        /// <summary>
        /// Generates a public and private ECDSA key pair with a random seed
        /// </summary>
        /// <param name="signatureAlgo"></param>
        /// <param name="hashAlgo"></param>
        /// <param name="weight"></param>
        /// <returns>A <see cref="FlowAccount"/></returns>
        public static FlowAccountKey GenerateRandomEcdsaKey(SignatureAlgo signatureAlgo, HashAlgo hashAlgo, uint weight = 1000)
        {
            var newKeys = Crypto.Ecdsa.Utilities.GenerateKeyPair(signatureAlgo);
            var publicKey = Crypto.Ecdsa.Utilities.DecodePublicKeyToHex(newKeys);
            var privateKey = Crypto.Ecdsa.Utilities.DecodePrivateKeyToHex(newKeys);

            return new FlowAccountKey
            {
                PrivateKey = privateKey,
                PublicKey = publicKey,
                SignatureAlgorithm = signatureAlgo,
                HashAlgorithm = hashAlgo,
                Weight = weight
            };
        }

        public static IList<FlowAccountKey> UpdateFlowAccountKeys(IList<FlowAccountKey> currentFlowAccountKeys, IList<FlowAccountKey> updatedFlowAccountKeys)
        {
            foreach (var key in updatedFlowAccountKeys)
            {
                var currentKey = currentFlowAccountKeys.FirstOrDefault(w => w.PublicKey == key.PublicKey);
                if (currentKey == null || string.IsNullOrEmpty(currentKey.PrivateKey))
                    continue;

                key.PrivateKey = currentKey.PrivateKey;
                key.Signer = Crypto.Ecdsa.Utilities.CreateSigner(key.PrivateKey, key.SignatureAlgorithm, key.HashAlgorithm);
            }

            return updatedFlowAccountKeys;
        }
    }
}
