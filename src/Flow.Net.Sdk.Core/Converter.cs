using Flow.Net.Sdk.Core.Cadence;
using Flow.Net.Sdk.Core.Constants;
using Flow.Net.Sdk.Core.Exceptions;
using Flow.Net.Sdk.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Flow.Net.Sdk.Core
{
    public static class Converter
    {
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

        public static string AddHexPrefix(string hex)
        {
            try
            {
                if (!hex.StartsWith("0x"))
                    hex = $"0x{hex}";

                return hex;
            }
            catch (Exception exception)
            {
                throw new FlowException("Failed to remove hex prefix", exception);
            }
        }

        /// <summary>
        /// Filters a <see cref="IEnumerable{T}" /> of type <see cref="FlowEvent"/> where <see cref="Type"/> is equal to "flow.AccountCreated" and returns a <see cref="FlowAddress"/>.
        /// </summary>
        /// <param name="flowEvents"></param>
        /// <returns>A <see cref="FlowAddress"/> that satisfies the condition.</returns>
        public static FlowAddress AccountCreatedAddress(IEnumerable<FlowEvent> flowEvents)
        {
            var accountCreatedEvent = flowEvents.FirstOrDefault(w => w.Type == Event.AccountCreated);

            if (accountCreatedEvent == null)
                return null;

            if (accountCreatedEvent.Payload == null)
                return null;

            var compositeItemFields = accountCreatedEvent.Payload.As<CadenceComposite>().Value.Fields.ToList();

            if (!compositeItemFields.Any())
                return null;

            var addressValue = compositeItemFields.FirstOrDefault();

            if (addressValue == null)
                return null;

            return new FlowAddress(addressValue.Value.As<CadenceAddress>().Value);
        }
    }
}
