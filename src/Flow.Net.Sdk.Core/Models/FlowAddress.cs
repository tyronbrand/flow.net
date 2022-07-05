namespace Flow.Net.Sdk.Core.Models
{
    public class FlowAddress
    {
        public FlowAddress(string address)
        {
            Address = address.RemoveHexPrefix();
        }

        public string Address { get; set; }
    }
}
