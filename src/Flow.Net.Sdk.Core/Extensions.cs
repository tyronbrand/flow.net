using Flow.Net.Sdk.Core.Cadence;
using Flow.Net.Sdk.Core.Models;
using System.Collections.Generic;

namespace Flow.Net.Sdk.Core
{
    public static class Extensions
    {        
        public static string BytesToHex(this byte[] data, bool include0x = false)
        {
            return Converter.BytesToHex(data, include0x);
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
    }
}
