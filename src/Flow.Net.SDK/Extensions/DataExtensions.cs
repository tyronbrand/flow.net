using Google.Protobuf;
using System;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Flow.Net.Sdk.Extensions
{
    public static class DataExtensions
    {
        public static ByteString FromHexToByteString(this string hex)
        {
            var numberChars = hex.Length;
            var bytes = new byte[numberChars / 2];

            for (var i = 0; i < numberChars; i += 2)
                bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);

            return ByteString.CopyFrom(bytes);
        }

        public static string FromByteStringToHex(this ByteString byteString)
        {
            var byteArray = byteString.ToByteArray();
            var hex = new StringBuilder(byteArray.Length * 2);

            foreach (var b in byteArray)
                hex.AppendFormat("{0:x2}", b);

            return hex.ToString();
        }

        public static string FromByteArrayToHex(this byte[] data, bool include0x = false)
        {
            return (include0x ? "0x" : string.Empty) + BitConverter.ToString(data).Replace("-", "").ToLower();
        }

        public static ByteString FromByteArrayToByteString(this byte[] bytes)
        {
            return ByteString.CopyFrom(bytes);
        }

        public static ByteString FromStringToByteString(this string str)
        {
            return ByteString.CopyFromUtf8(str);
        }

        public static string FromByteStringToString(this ByteString str)
        {
            return str.ToStringUtf8();
        }

        public static string FromStringToHex(this string str)
        {
            return Encoding.UTF8.GetBytes(str).FromByteArrayToHex();
        }

        public static byte[] FromHexToBytes(this string hex)
        {
            if (hex.Substring(0, 2).ToLower() == "0x")
                hex = hex.ToLower().Replace("0x", string.Empty);

            if (hex.IsHexString())
            {
                return Enumerable.Range(0, hex.Length)
                    .Where(x => x % 2 == 0)
                    .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
                    .ToArray();
            }
            else
            {
                throw new Exception("invalid hex string");
            }
        }

        public static bool IsHexString(this string str)
        {
            if (str.Length == 0)
                return false;

            if (str.Substring(0, 2).ToLower() == "0x")
                str = str.ToLower().Replace("0x", string.Empty);

            var regex = new Regex(@"^[0-9a-f]+$");
            return regex.IsMatch(str) && str.Length % 2 == 0;
        }

        public static string Remove0x(this string value)
        {
            if (value.Substring(0, 2).ToLower() == "0x")
                value = value.ToLower().Replace("0x", string.Empty);

            return value;
        }
    }
}
