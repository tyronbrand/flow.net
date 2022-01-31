using System.Collections.Generic;

namespace Flow.Net.Sdk.Constants
{
    public static class Defaults
    {
        public static Dictionary<string, string> EmulatorAddresses => new Dictionary<string, string>()
        {
            { "FlowToken", "0x0ae53cb6e3f42a79" },
            { "FungibleToken", "0xee82856bf20e2aa6" },
            { "FlowFees", "0xe5a8b7f23e8b548f" },
            { "FlowStorageFees", "0xf8d6e0586b0a20c7" }
        };
    }
}