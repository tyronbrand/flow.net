using Flow.Net.Sdk.Core.Cadence;

namespace Flow.Net.Sdk.Core
{
    public static class Extensions
    {        
        public static string FromByteArrayToHex(this byte[] data, bool include0x = false)
        {
            return Converter.FromByteArrayToHex(data, include0x);
        }        

        public static string FromStringToHex(this string str)
        {
            return Converter.FromStringToHex(str);
        }

        public static byte[] FromHexToBytes(this string hex)
        {
            return Converter.FromHexToBytes(hex);
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
    }
}
