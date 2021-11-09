using Flow.Net.Sdk.Converters;
using Xunit;

namespace Flow.Net.Sdk.Tests
{
    public class ConverterTest
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

        [Fact]
        public void TestFromHexToByteString0xPrefix()
        {
            const string expectedResult = "0102030405060708";

            const string hexWith0x = "0x0102030405060708";
            var removed0xResult = ByteStringConverter.FromHexToByteString(hexWith0x);

            const string hexWithout0x = "0102030405060708";
            var result = ByteStringConverter.FromHexToByteString(hexWithout0x);

            Assert.Equal(expectedResult, removed0xResult.FromByteStringToHex());
            Assert.Equal(expectedResult, result.FromByteStringToHex());
        }

        [Fact]
        public void TestRemoveHexPrefix()
        {
            const string expectedResult = "0102030405060708";
            const string hexWith0x = "0x0102030405060708";

            var removed0xResult = HexConverter.RemoveHexPrefix(hexWith0x);
            var result = HexConverter.RemoveHexPrefix(expectedResult);

            Assert.Equal(expectedResult, removed0xResult);
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public void TestFromStringToHex()
        {
            const string expectedResult = "0102030405060708";
            const string str = "\u0001\u0002\u0003\u0004\u0005\u0006\a\b";

            var result = HexConverter.FromStringToHex(str);

            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public void TestFromHexToBytes()
        {
            var expectedResult = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 };
            const string hex = "0102030405060708";

            var result = HexConverter.FromHexToBytes(hex);

            Assert.Equal(expectedResult, result);
        }
    }
}
