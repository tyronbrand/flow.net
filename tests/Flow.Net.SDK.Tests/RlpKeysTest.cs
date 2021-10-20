using Flow.Net.Sdk.Extensions;
using Flow.Net.Sdk.Models;
using Flow.Net.Sdk.RecursiveLengthPrefix;
using System.Collections.Generic;
using Xunit;

namespace Flow.Net.Sdk.Tests
{
    public class RlpKeysTest
    {
        [Fact]
        public void TestKeyEncoding()
        {
            var keyItems = new List<KeyEncodingTestItem>
            {
                new KeyEncodingTestItem
                {
                    SignatureAlgorithm = SignatureAlgo.ECDSA_P256,
                    HashAlgorithm = HashAlgo.SHA3_256,
                    Weight = 0,
                    PublicKey = "0bfcd8790c3ce88f3fac9d4bd23514f48bf0cdd1f6c3c8bdf87b11489b1bbeca1ef805ec2ee76451e9bdb265284f78febaeacbc8b0827e0a7baafee4e655d0b5",
                    ExpectedResult = "f845b8400bfcd8790c3ce88f3fac9d4bd23514f48bf0cdd1f6c3c8bdf87b11489b1bbeca1ef805ec2ee76451e9bdb265284f78febaeacbc8b0827e0a7baafee4e655d0b5020380"
                },
                new KeyEncodingTestItem
                {
                    SignatureAlgorithm = SignatureAlgo.ECDSA_P256,
                    HashAlgorithm = HashAlgo.SHA3_256,
                    Weight = 32,
                    PublicKey = "0bfcd8790c3ce88f3fac9d4bd23514f48bf0cdd1f6c3c8bdf87b11489b1bbeca1ef805ec2ee76451e9bdb265284f78febaeacbc8b0827e0a7baafee4e655d0b5",
                    ExpectedResult = "f845b8400bfcd8790c3ce88f3fac9d4bd23514f48bf0cdd1f6c3c8bdf87b11489b1bbeca1ef805ec2ee76451e9bdb265284f78febaeacbc8b0827e0a7baafee4e655d0b5020320"
                },
                new KeyEncodingTestItem
                {
                    SignatureAlgorithm = SignatureAlgo.ECDSA_secp256k1,
                    HashAlgorithm = HashAlgo.SHA3_256,
                    Weight = 0,
                    PublicKey = "0bfcd8790c3ce88f3fac9d4bd23514f48bf0cdd1f6c3c8bdf87b11489b1bbeca1ef805ec2ee76451e9bdb265284f78febaeacbc8b0827e0a7baafee4e655d0b5",
                    ExpectedResult = "f845b8400bfcd8790c3ce88f3fac9d4bd23514f48bf0cdd1f6c3c8bdf87b11489b1bbeca1ef805ec2ee76451e9bdb265284f78febaeacbc8b0827e0a7baafee4e655d0b5030380"
                },
                new KeyEncodingTestItem
                {
                    SignatureAlgorithm = SignatureAlgo.ECDSA_P256,
                    HashAlgorithm = HashAlgo.SHA2_256,
                    Weight = 0,
                    PublicKey = "0bfcd8790c3ce88f3fac9d4bd23514f48bf0cdd1f6c3c8bdf87b11489b1bbeca1ef805ec2ee76451e9bdb265284f78febaeacbc8b0827e0a7baafee4e655d0b5",
                    ExpectedResult = "f845b8400bfcd8790c3ce88f3fac9d4bd23514f48bf0cdd1f6c3c8bdf87b11489b1bbeca1ef805ec2ee76451e9bdb265284f78febaeacbc8b0827e0a7baafee4e655d0b5020180"
                }
            };

            foreach(var item in keyItems)
            {
                var key = new FlowAccountKey
                {
                    HashAlgorithm = item.HashAlgorithm,
                    SignatureAlgorithm = item.SignatureAlgorithm,
                    PublicKey = item.PublicKey,
                    Weight = item.Weight
                };

                var encoded = Rlp.EncodedAccountKey(key).FromByteArrayToHex();

                Assert.Equal(item.ExpectedResult, encoded);
            }
        }

        public class KeyEncodingTestItem : FlowAccountKey
        {
            public string ExpectedResult { get; set; }
        }
    }
}
