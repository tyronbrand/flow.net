using Flow.Net.Sdk.Models;
using Xunit;

namespace Flow.Net.Sdk.Tests
{
    public  class FlowAddressTest
    {
        [Fact]
        public void TestFlowAddressHexInputWith0x()
        {
            const string input = "0x11111111";
            const string expectedOutput = "11111111";

            var flowAddress = new FlowAddress(input);

            Assert.Equal(expectedOutput, flowAddress.HexValue);
            Assert.Equal(expectedOutput.FromHexToByteString(), flowAddress.Value);
        }

        [Fact]
        public void TestFlowAddressHexInputWithout0x()
        {
            const string input = "11111111";
            const string expectedOutput = "11111111";

            var flowAddress = new FlowAddress(input);

            Assert.Equal(expectedOutput, flowAddress.HexValue);
            Assert.Equal(expectedOutput.FromHexToByteString(), flowAddress.Value);
        }

        [Fact]
        public void TestFlowAddressByteStringInputWith0x()
        {
            var input = "0x11111111".FromHexToByteString();
            var expectedOutput = "11111111".FromHexToByteString();

            var flowAddress = new FlowAddress(input);

            Assert.Equal(expectedOutput, flowAddress.HexValue.FromHexToByteString());
            Assert.Equal(expectedOutput, flowAddress.Value);
        }

        [Fact]
        public void TestFlowAddressByteStringInputWithout0x()
        {
            var input = "11111111".FromHexToByteString();
            var expectedOutput = "11111111".FromHexToByteString();

            var flowAddress = new FlowAddress(input);

            Assert.Equal(expectedOutput, flowAddress.HexValue.FromHexToByteString());
            Assert.Equal(expectedOutput, flowAddress.Value);
        }
    }
}
