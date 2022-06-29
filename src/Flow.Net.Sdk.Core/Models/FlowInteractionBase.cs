using Flow.Net.Sdk.Core.Cadence;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Flow.Net.Sdk.Core.Models
{
    public abstract class FlowInteractionBase
    {
        private readonly Dictionary<string, string> _addressMap;

        protected FlowInteractionBase(Dictionary<string, string> addressMap = null)
        {
            _addressMap = addressMap;
        }

        private string _script;
        public string Script
        {
            get => _script;
            set => _script = _addressMap != null ? ReplaceImports(value, _addressMap) : value;
        }

        private static string ReplaceImports(string txText, Dictionary<string, string> addressMap)
        {
            const string pattern = @"^(\s*import\s+\w+\s+from\s+)(?:0x)?(\w+)\s*$";
            return string.Join("\n",
                txText.Split('\n')
                .Select(line =>
                {
                    var match = Regex.Match(line, pattern);
                    //if (match.Success && match.Groups.Count == 3)
                    //{
                    //    var key = match.Groups[2].Value;
                    //    var replAddress = addressMap.GetValueOrDefault(key)
                    //        ?? addressMap.GetValueOrDefault($"0x{key}");
                    //    if (!string.IsNullOrEmpty(replAddress))
                    //    {
                    //        replAddress = replAddress.TrimStart("0x");
                    //        return $"{match.Groups[1].Value}0x{replAddress}";
                    //    }
                    //}
                    return line;
                }));
        }

        public IList<ICadence> Arguments { get; set; } = new List<ICadence>();
    }
}
