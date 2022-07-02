using Flow.Net.Sdk.Core.Cadence;
using Flow.Net.Sdk.Core.Models;
using System.Collections.Generic;
using System.Linq;

namespace Flow.Net.Sdk.Core
{
    public static class Extensions
    {        
        public static string BytesToHex(this byte[] data, bool include0X = false)
        {
            return Converter.BytesToHex(data, include0X);
        }        

        public static string StringToHex(this string str)
        {
            return Converter.StringToHex(str);
        }

        public static byte[] HexToBytes(this string hex)
        {
            return Converter.HexToBytes(hex);
        }

        public static bool IsHexString(this string str)
        {
            return Converter.IsHexString(str);
        }

        public static string RemoveHexPrefix(this string hex)
        {
            return Converter.RemoveHexPrefix(hex);
        }

        public static string AddHexPrefix(this string hex)
        {
            return Converter.AddHexPrefix(hex);
        }

        public static CadenceHashAlgorithm FromHashAlgoToCadenceHashAlgorithm(this HashAlgo hashAlgo)
        {
            return CadenceExtensions.FromHashAlgoToCadenceHashAlgorithm(hashAlgo);
        }

        public static CadenceSignatureAlgorithm FromSignatureAlgoToCadenceSignatureAlgorithm(this SignatureAlgo signatureAlgo)
        {
            return CadenceExtensions.FromSignatureAlgoToCadenceSignatureAlgorithm(signatureAlgo);
        }

        ///<inheritdoc cref="Converter.AccountCreatedAddress"/>
        public static FlowAddress AccountCreatedAddress(this IEnumerable<FlowEvent> flowEvents)
        {
            return Converter.AccountCreatedAddress(flowEvents);
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
