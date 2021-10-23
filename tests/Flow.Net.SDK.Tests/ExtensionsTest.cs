using Xunit;

namespace Flow.Net.Sdk.Tests
{
    public class ExtensionsTest
    {
        [Fact]
        public void TestBytesToHex()
        {
            var bytes = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 };

            var without0x = bytes.FromByteArrayToHex();
            var with0x = bytes.FromByteArrayToHex(true);

            Assert.Equal("0102030405060708", without0x);
            Assert.Equal("0x0102030405060708", with0x);
        }
    }
}
