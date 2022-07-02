namespace Flow.Net.Sdk.Core.Tests
{
    public  class FlowAccountTest
    {
        [Fact]
        public void TestAccountKey()
        {
            uint weight = 500;
            var signatureAlgo = SignatureAlgo.ECDSA_P256;
            var hashAlgo = HashAlgo.SHA3_256;
            var accountKey = FlowAccountKey.GenerateRandomEcdsaKey(signatureAlgo, hashAlgo, weight);

            Assert.Equal(signatureAlgo, accountKey.SignatureAlgorithm);
            Assert.Equal(hashAlgo, accountKey.HashAlgorithm);
            Assert.Equal(weight, accountKey.Weight);
            Assert.Equal((ulong)0, accountKey.SequenceNumber);
            Assert.Equal((uint)0, accountKey.Index);
        }
    }
}
