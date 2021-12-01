namespace Flow.Net.Sdk
{
    public static class DomainTag
    {
        private static byte[] MessageWithDomain(byte[] bytes, byte[] domain)
        {
            return Utilities.CombineByteArrays(new[]
            {
                domain,
                bytes
            });
        }

        public static byte[] AddUserDomainTag(byte[] bytes)
        {
            var userTag = Utilities.Pad("FLOW-V0.0-user", 32, false);
            return MessageWithDomain(bytes, userTag);
        }

        public static byte[] AddTransactionDomainTag(byte[] bytes)
        {
            var domainTag = Utilities.Pad("FLOW-V0.0-transaction", 32, false);
            return MessageWithDomain(bytes, domainTag);
        }
    }
}
