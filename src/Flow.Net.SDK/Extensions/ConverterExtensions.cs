using Flow.Net.Sdk.Converters;
using Google.Protobuf;

namespace Flow.Net.Sdk
{
    public static class ConverterExtensions
    {
        public static ByteString FromHexToByteString(this string hex)
        {
            return ByteStringConverter.FromHexToByteString(hex);
        }

        public static string FromByteStringToHex(this ByteString byteString)
        {
            return HexConverter.FromByteStringToHex(byteString);
        }

        public static string FromByteArrayToHex(this byte[] data, bool include0x = false)
        {
            return HexConverter.FromByteArrayToHex(data, include0x);
        }

        public static ByteString FromByteArrayToByteString(this byte[] bytes)
        {
            return ByteStringConverter.FromByteArrayToByteString(bytes);
        }

        public static ByteString FromStringToByteString(this string str)
        {
            return ByteStringConverter.FromStringToByteString(str);
        }

        public static string FromByteStringToString(this ByteString str)
        {
            return ByteStringConverter.FromByteStringToString(str);
        }

        public static string FromStringToHex(this string str)
        {
            return HexConverter.FromStringToHex(str);
        }

        public static byte[] FromHexToBytes(this string hex)
        {
            return HexConverter.FromHexToBytes(hex);
        }

        public static bool IsHexString(this string str)
        {
            return HexConverter.IsHexString(str);
        }

        public static string RemoveHexPrefix(this string hex)
        {
            return HexConverter.RemoveHexPrefix(hex);
        }
    }
}
