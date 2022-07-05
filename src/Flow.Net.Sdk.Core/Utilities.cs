using Flow.Net.Sdk.Core.Models;
using Newtonsoft.Json;
using Org.BouncyCastle.Crypto.Parameters;
using System;
using System.IO;
using System.Linq;
using System.Text;

namespace Flow.Net.Sdk.Core
{
    public static class Utilities
    {
        public static FlowConfig ReadConfig(string fileName = null, string path = null)
        {
            if (string.IsNullOrEmpty(fileName))
                fileName = "flow";

            if (string.IsNullOrEmpty(path))
                path = AppContext.BaseDirectory;

            var file = File.ReadAllText(Path.Combine(path, $"{fileName}.json"));
            return JsonConvert.DeserializeObject<FlowConfig>(file);
        }

        public static FlowAccount AddSignerFromConfigAccount(FlowConfigAccount flowConfigAccount, FlowAccount flowAccount)
        {
            if(flowAccount.Keys != null)
            {
                foreach (var key in flowAccount.Keys)
                {
                    // getting the public key so we can match it to our account public keys
                    var keyPair = Crypto.Ecdsa.Utilities.AsymmetricCipherKeyPairFromPrivateKey(flowConfigAccount.Key, key.SignatureAlgorithm);
                    var publicKey = Crypto.Ecdsa.Utilities.DecodePublicKeyToHex(keyPair);

                    // select the key with a matching public key
                    var flowAccountKey = flowAccount.Keys.FirstOrDefault(w => w.PublicKey == publicKey);

                    if (flowAccountKey == null)
                        continue;

                    flowAccountKey.PrivateKey = key.PrivateKey;

                    var privateKey = keyPair.Private as ECPrivateKeyParameters;
                    flowAccountKey.Signer = new Crypto.Ecdsa.Signer(privateKey, flowAccountKey.HashAlgorithm, flowAccountKey.SignatureAlgorithm);
                }
            }            

            return flowAccount;
        }

        public static string ReadCadenceScript(string fileName, string path = null)
        {
            if (string.IsNullOrEmpty(path))
                path = Path.Combine(AppContext.BaseDirectory, "Cadence");

            return File.ReadAllText(Path.Combine(path, $"{fileName}.cdc"));
        }

        public static byte[] Pad(string tag, int length, bool padLeft = true)
        {
            var bytes = Encoding.UTF8.GetBytes(tag);

            return padLeft ? bytes.PadLeft(length) : bytes.PadRight(length);
        }

        public static byte[] Pad(byte[] bytes, int length, bool padLeft = true)
        {
            return padLeft ? bytes.PadLeft(length) : bytes.PadRight(length);
        }

        private static byte[] PadLeft(this byte[] bytes, int length)
        {
            if (bytes.Length >= length)
                return bytes;

            var newArray = new byte[length];
            Array.Copy(bytes, 0, newArray, newArray.Length - bytes.Length, bytes.Length);
            return newArray;
        }

        private static byte[] PadRight(this byte[] bytes, int length)
        {
            if (bytes.Length >= length)
                return bytes;

            Array.Resize(ref bytes, length);
            return bytes;
        }

        public static byte[] CombineByteArrays(byte[][] arrays)
        {
            var rv = new byte[arrays.Sum(a => a.Length)];
            var offset = 0;
            foreach (byte[] array in arrays)
            {
                Buffer.BlockCopy(array, 0, rv, offset, array.Length);
                offset += array.Length;
            }
            return rv;
        }

        public static byte[] SerializeObject(object value)
        {
            return Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(value));
        }
    }
}
