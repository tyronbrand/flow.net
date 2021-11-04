using Flow.Net.Sdk.Exceptions;
using Google.Protobuf;
using System;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Flow.Net.Sdk.Converters
{
    public class HexConverter
    {
        public static string FromByteStringToHex(ByteString byteString)
        {
            try
            {
                var byteArray = byteString.ToByteArray();
                var hex = new StringBuilder(byteArray.Length * 2);

                foreach (var b in byteArray)
                    hex.AppendFormat("{0:x2}", b);

                return hex.ToString();
            }
            catch(Exception exception)
            {
                throw new FlowException("Failed to convert ByteString to hex.", exception);
            }            
        }

        public static string FromByteArrayToHex(byte[] data, bool include0x = false)
        {
            try
            {
                return (include0x ? "0x" : string.Empty) + BitConverter.ToString(data).Replace("-", "").ToLower();
            }
            catch (Exception exception)
            {
                throw new FlowException("Failed to convert byte[] to hex.", exception);
            }            
        }

        public static string FromStringToHex(string str)
        {
            try
            {
                return FromByteArrayToHex(Encoding.UTF8.GetBytes(str));
            }
            catch (Exception exception)
            {
                throw new FlowException("Failed to convert string to hex.", exception);
            }            
        }

        public static byte[] FromHexToBytes(string hex)
        {
            try
            {
                hex = RemoveHexPrefix(hex);

                if (IsHexString(hex))
                {
                    return Enumerable.Range(0, hex.Length)
                        .Where(x => x % 2 == 0)
                        .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
                        .ToArray();
                }

                throw new FlowException("Invalid hex string.");
            }
            catch (Exception exception)
            {
                throw new FlowException("Failed to convert hex to byte[].", exception);
            }        
        }

        public static bool IsHexString(string str)
        {
            try
            {
                if (str.Length == 0)
                    return false;

                str = RemoveHexPrefix(str);

                var regex = new Regex(@"^[0-9a-f]+$");
                return regex.IsMatch(str) && str.Length % 2 == 0;
            }
            catch (Exception exception)
            {
                throw new FlowException("Failed to determine if string is hex.", exception);
            }            
        }

        public static string RemoveHexPrefix(string hex)
        {
            try
            {
                return hex.Substring(hex.StartsWith("0x") ? 2 : 0);
            }
            catch (Exception exception)
            {
                throw new FlowException("Failed to remove hex prefix", exception);
            }            
        }
    }
}
