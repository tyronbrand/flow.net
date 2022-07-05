using Flow.Net.Sdk.Core;
using Flow.Net.Sdk.Core.Exceptions;
using Google.Protobuf;
using System;
using System.Text;

namespace Flow.Net.Sdk.Client.Grpc
{
    public static class Extensions
    {
        public static string ByteStringToHex(this ByteString byteString)
        {
            try
            {
                var byteArray = byteString.ToByteArray();
                var hex = new StringBuilder(byteArray.Length * 2);

                foreach (var b in byteArray)
                    hex.AppendFormat("{0:x2}", b);

                return hex.ToString();
            }
            catch (Exception exception)
            {
                throw new FlowException("Failed to convert ByteString to hex.", exception);
            }
        }

        public static ByteString HexToByteString(this string hex)
        {
            try
            {
                hex = hex.RemoveHexPrefix();

                var numberChars = hex.Length;
                var bytes = new byte[numberChars / 2];

                for (var i = 0; i < numberChars; i += 2)
                    bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);

                return ByteString.CopyFrom(bytes);
            }
            catch (Exception exception)
            {
                throw new FlowException("Failed to convert hex to ByteString.", exception);
            }
        }

        public static ByteString BytesToByteString(this byte[] bytes)
        {
            try
            {
                return ByteString.CopyFrom(bytes);
            }
            catch (Exception exception)
            {
                throw new FlowException("Failed to convert byte[] to ByteString.", exception);
            }
        }

        public static ByteString StringToByteString(this string str)
        {
            try
            {
                return ByteString.CopyFromUtf8(str);
            }
            catch (Exception exception)
            {
                throw new FlowException("Failed to convert string to ByteString.", exception);
            }
        }

        public static string ByteStringToString(this ByteString str)
        {
            try
            {
                return str.ToStringUtf8();
            }
            catch (Exception exception)
            {
                throw new FlowException("Failed to convert ByteString to string.", exception);
            }
        }
    }
}
