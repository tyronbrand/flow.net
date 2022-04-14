using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Flow.Net.Sdk.Models
{
    public class FlowCadenceScript
    {
        private readonly Dictionary<string, string> _addressMap;

        public FlowCadenceScript(Dictionary<string, string> AddressMap = null)
        {
            _addressMap = AddressMap;
        }

        private string _script;

        public string Script
        {
            get
            {
                return _script;
            }

            set
            {
                if (_addressMap != null)
                {
                    _script = ReplaceImports(value, _addressMap);
                }
                else
                {
                    _script = value;
                }
            }
        }

        private static string ReplaceImports(string txText, Dictionary<string, string> addressMap)
        {
            var pattern = @"^(\s*import\s+\w+\s+from\s+)(?:0x)?(\w+)\s*$";
            return string.Join("\n",
                txText.Split('\n')
                .Select(line =>
                {
                    var match = Regex.Match(line, pattern);
                    if (match.Success && match.Groups.Count == 3)
                    {
                        var key = match.Groups[2].Value;
                        var replAddress = addressMap.GetValueOrDefault(key)
                            ?? addressMap.GetValueOrDefault($"0x{key}");
                        if (!string.IsNullOrEmpty(replAddress))
                        {
                            replAddress = replAddress.TrimStart("0x");
                            return $"{match.Groups[1].Value}0x{replAddress}";
                        }
                    }
                    return line;
                }));
        }
    }
}
