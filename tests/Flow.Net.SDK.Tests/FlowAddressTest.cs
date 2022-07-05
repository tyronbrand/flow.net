using Flow.Net.Sdk.Core.Models;
using Flow.Net.Sdk.Client.Grpc;
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

            Assert.Equal(expectedOutput, flowAddress.Address);
        }

        [Fact]
        public void TestFlowAddressHexInputWithout0x()
        {
            const string input = "11111111";
            const string expectedOutput = "11111111";

            var flowAddress = new FlowAddress(input);

            Assert.Equal(expectedOutput, flowAddress.Address);
        }

        [Fact]
        public void TestFlowAddressByteStringInputWith0x()
        {
            var input = "0x11111111".HexToByteString();
            var expectedOutput = "11111111".HexToByteString();

            var flowAddress = new FlowAddress(input.ByteStringToHex());

            Assert.Equal(expectedOutput, flowAddress.Address.HexToByteString());
        }

        [Fact]
        public void TestFlowAddressByteStringInputWithout0x()
        {
            var input = "11111111".HexToByteString();
            var expectedOutput = "11111111".HexToByteString();

            var flowAddress = new FlowAddress(input.ByteStringToHex());

            Assert.Equal(expectedOutput, flowAddress.Address.HexToByteString());
        }
    }
}
