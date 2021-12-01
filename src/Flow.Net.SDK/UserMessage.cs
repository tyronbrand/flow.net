using Flow.Net.Sdk.Crypto;

namespace Flow.Net.Sdk
{
    public static class UserMessage
    {
        public static byte[] Sign(byte[] message, ISigner signer)
        {
            message = DomainTag.AddUserDomainTag(message);
            return signer.Sign(message);
        }
    }
}
