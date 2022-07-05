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
    public static class Extensions
    {        
        public static string BytesToHex(this byte[] data, bool include0X = false)
        {
            try
            {
                return (include0X ? "0x" : string.Empty) + BitConverter.ToString(data).Replace("-", "").ToLower();
            }
            catch (Exception exception)
            {
                throw new FlowException("Failed to convert byte[] to hex.", exception);
            }
        }        

        public static string StringToHex(this string str)
        {
            try
            {
                return BytesToHex(Encoding.UTF8.GetBytes(str));
            }
            catch (Exception exception)
            {
                throw new FlowException("Failed to convert string to hex.", exception);
            }
        }

        public static byte[] HexToBytes(this string hex)
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

        public static bool IsHexString(this string str)
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

        public static string RemoveHexPrefix(this string hex)
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

        public static string AddHexPrefix(this string hex)
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

        public static CadenceHashAlgorithm FromHashAlgoToCadenceHashAlgorithm(this HashAlgo hashAlgo)
        {
            return CadenceExtensions.FromHashAlgoToCadenceHashAlgorithm(hashAlgo);
        }

        public static CadenceSignatureAlgorithm FromSignatureAlgoToCadenceSignatureAlgorithm(this SignatureAlgo signatureAlgo)
        {
            return CadenceExtensions.FromSignatureAlgoToCadenceSignatureAlgorithm(signatureAlgo);
        }

        /// <summary>
        /// Filters a <see cref="IEnumerable{T}" /> of type <see cref="FlowEvent"/> where <see cref="Type"/> is equal to "flow.AccountCreated" and returns a <see cref="FlowAddress"/>.
        /// </summary>
        /// <param name="flowEvents"></param>
        /// <returns>A <see cref="FlowAddress"/> that satisfies the condition.</returns>
        public static FlowAddress AccountCreatedAddress(this IEnumerable<FlowEvent> flowEvents)
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

        /// <summary>
        /// Removes string "start" from beginning of the string
        /// </summary>
        /// <param name="s">string</param>
        /// <param name="start">string to trim</param>
        public static string TrimStart(this string s, string start)
        {
            return s.StartsWith(start)
                ? s.Substring(start.Length)
                : s;
        }

        public static TV GetValueOrDefault<TK, TV>(this Dictionary<TK, TV> x, TK key)
        {
            return x.TryGetValue(key, out var value) ? value : default;
        }

        /// <summary>
        /// Merge dictionaries where collisions favor the second dictionary keys
        /// </summary>
        public static Dictionary<TK, TV> Merge<TK, TV>(this Dictionary<TK, TV> firstDict,
            Dictionary<TK, TV> secondDict)
        {
            return secondDict
                .Concat(firstDict)
                .GroupBy(d => d.Key)
                .ToDictionary(x => x.Key, x => x.First().Value);
        }
    }
}
