using Org.BouncyCastle.Crypto.Digests;
using System;
using System.Security.Cryptography;

namespace Flow.Net.Sdk.Core.Crypto
{
    public static class Hasher
    {
        public static byte[] CalculateHash(byte[] bytes, HashAlgo hashAlgo)
        {
            switch (hashAlgo)
            {
                case HashAlgo.SHA2_256:
                    return HashSha2_256(bytes);
                case HashAlgo.SHA3_256:
                    return HashSha3(bytes, 256);
                default:
                    throw new ArgumentOutOfRangeException(nameof(hashAlgo), hashAlgo, null);
            }
        }

        private static byte[] HashSha2_256(byte[] bytes)
        {
            using (var sha256Hash = SHA256.Create())
                return sha256Hash.ComputeHash(bytes);
        }

        private static byte[] HashSha3(byte[] bytes, int bitLength)
        {
            var hashAlgorithm = new Sha3Digest(bitLength);
            var result = new byte[hashAlgorithm.GetDigestSize()];
            hashAlgorithm.BlockUpdate(bytes, 0, bytes.Length);
            hashAlgorithm.DoFinal(result, 0);
            return result;
        }
    }
}
