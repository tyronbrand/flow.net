using Xunit;

namespace Flow.Net.Sdk.Tests
{
    public class HashingTest
    {
        [Fact]
        public void TestSHA3_256Hashing()
        {
            const string input = "f882f872b07472616e73616374696f6e207b2065786563757465207b206c6f67282248656c6c6f2c20576f726c64212229207d207dc0a001020000000000000000000000000000000000000000000000000000000000002a88f8d6e0586b0a20c7032a88ee82856bf20e2aa6c988f8d6e0586b0a20c7c8c3800202c3800301c4c3010703";
            const string expectedOutput = "0xd1a2c58aebfce1050a32edf3568ec3b69cb8637ae090b5f7444ca6b2a8de8f8b";

            var output = Crypto.Hasher.CalculateHash(input.FromHexToBytes(), HashAlgo.SHA3_256);
            Assert.Equal(expectedOutput, output.FromByteArrayToHex(true));
        }

        [Fact]
        public void TestSHA2_256Hashing()
        {
            const string input = "f882f872b07472616e73616374696f6e207b2065786563757465207b206c6f67282248656c6c6f2c20576f726c64212229207d207dc0a001020000000000000000000000000000000000000000000000000000000000002a88f8d6e0586b0a20c7032a88ee82856bf20e2aa6c988f8d6e0586b0a20c7c8c3800202c3800301c4c3010703";
            const string expectedOutput = "0x19dde0c3aa410c01cf7ed505acd62759a83ad10518920e012e2001aec47d84c1";

            var output = Crypto.Hasher.CalculateHash(input.FromHexToBytes(), HashAlgo.SHA2_256);
            Assert.Equal(expectedOutput, output.FromByteArrayToHex(true));
        }
    }
}
