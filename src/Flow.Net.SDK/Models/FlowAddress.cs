using Google.Protobuf;

namespace Flow.Net.Sdk.Models
{
    /// <summary>
    /// FlowAddress represents the address of an account.
    /// </summary>
    public class FlowAddress
    {
        public FlowAddress(ByteString address)
        {
            Value = address.FromByteStringToHex().FromHexToByteString();
            HexValue = address.FromByteStringToHex();
        }

        public FlowAddress(string addressHex)
        {
            Value = addressHex.FromHexToByteString();
            HexValue = addressHex.RemoveHexPrefix();
        }

        public ByteString Value { get; private set; }
        public string HexValue { get; private set; }
    }
}
