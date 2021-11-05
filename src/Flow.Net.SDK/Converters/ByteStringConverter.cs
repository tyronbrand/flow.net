using Flow.Net.Sdk.Exceptions;
using Google.Protobuf;
using System;

namespace Flow.Net.Sdk.Converters
{
    /// <summary>
    /// Converts data types to ByteString.
    /// </summary>
    public class ByteStringConverter
    {
        public static ByteString FromHexToByteString(string hex)
        {
            try
            {
                hex = HexConverter.RemoveHexPrefix(hex);

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

        public static ByteString FromByteArrayToByteString(byte[] bytes)
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

        public static ByteString FromStringToByteString(string str)
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

        public static string FromByteStringToString(ByteString str)
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
