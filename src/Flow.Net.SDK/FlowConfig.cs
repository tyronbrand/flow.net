using System.Collections.Generic;

namespace Flow.Net.Sdk
{
    public class FlowConfig
    {
        public IDictionary<string, FlowConfigAccount> Accounts { get; set; }
        public IDictionary<string, string> Networks { get; set; }
        public IDictionary<string, string> Contracts { get; set; }
    }

    public class FlowConfigAccount
    {
        public string Address { get; set; }
        public string Key { get; set; }
    }
}
