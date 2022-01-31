using System.Collections.Generic;
using System.Linq;

namespace Flow.Net.Sdk
{
    public static class UtilityExtensions
    {

        /// <summary>
        /// Removes string "start" from beginning of the string
        /// </summary>
        /// <param name="start">string to trim</param>
        public static string TrimStart(this string s, string start)
        {
            return s.StartsWith(start)
                ? s.Substring(start.Length)
                : s;
        }

        public static V GetValueOrDefault<K, V>(this Dictionary<K, V> x, K key)
        {
            if(x.TryGetValue(key, out V value))
                return value;
            return default;
        }

        /// <summary>
        /// Merge dictionaries where collisions favor the second dictionary keys
        /// </summary>
        public static Dictionary<K, V> Merge<K, V>(this Dictionary<K, V> firstDict,
            Dictionary<K, V> secondDict)
        {
            return secondDict
                .Concat(firstDict)
                .GroupBy(d => d.Key)
                .ToDictionary(x => x.Key, x => x.First().Value);
        }
    }
}