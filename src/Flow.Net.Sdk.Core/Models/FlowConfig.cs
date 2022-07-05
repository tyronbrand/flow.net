using System.Collections.Generic;

namespace Flow.Net.Sdk.Core.Models
{
    /// <summary>
    /// FlowConfig aids reading of flow.json
    /// </summary>
    public class FlowConfig
    {
        public IDictionary<string, FlowConfigAccount> Accounts { get; set; }
        public IDictionary<string, string> Networks { get; set; }
        public IDictionary<string, string> Contracts { get; set; }
    }

    /// <summary>
    /// FlowConfigAccount represents an account
    /// </summary>
    public class FlowConfigAccount
    {
        public string Address { get; set; }
        public string Key { get; set; }
    }
}
