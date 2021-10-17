using Newtonsoft.Json;
using System;
using System.IO;
using System.Text;

namespace Flow.Net.Sdk
{
    public static class Utilities
    {
        public static FlowConfig ReadConfig(string fileName = null, string path = null)
        {
            if (string.IsNullOrEmpty(fileName))
                fileName = "flow";

            if (string.IsNullOrEmpty(path))
                path = AppContext.BaseDirectory;

            var file = File.ReadAllText($"{path}\\{fileName}.json");
            return JsonConvert.DeserializeObject<FlowConfig>(file);
        }

        public static string ReadCadenceScript(string fileName, string path = null)
        {
            if (string.IsNullOrEmpty(path))
                path = $"{AppContext.BaseDirectory}\\Cadence";

            return File.ReadAllText($"{path}\\{fileName}.cdc");
        }

        public static byte[] Pad(string tag, int length, bool padLeft = true)
        {
            var bytes = Encoding.UTF8.GetBytes(tag);

            if (padLeft)
                return bytes.PadLeft(length);
            else
                return bytes.PadRight(length);
        }

        public static byte[] Pad(byte[] bytes, int length, bool padLeft = true)
        {
            if (padLeft)
                return bytes.PadLeft(length);
            else
                return bytes.PadRight(length);
        }

        public static byte[] PadLeft(this byte[] bytes, int length)
        {
            if (bytes.Length >= length)
                return bytes;

            var newArray = new byte[length];
            Array.Copy(bytes, 0, newArray, (newArray.Length - bytes.Length), bytes.Length);
            return newArray;
        }

        public static byte[] PadRight(this byte[] bytes, int length)
        {
            if (bytes.Length >= length)
                return bytes;

            Array.Resize(ref bytes, length);
            return bytes;
        }
    }
}
