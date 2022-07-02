using Flow.Net.Sdk.Core.Crypto;

namespace Flow.Net.Sdk.Core
{
    public static class UserMessage
    {
        /// <summary>
        /// Signs the full user message (UserDomainTag + message).
        /// </summary>
        /// <param name="message"></param>
        /// <param name="signer"></param>
        /// <returns>Signed message as <see cref="byte" />[].</returns>
        public static byte[] Sign(byte[] message, ISigner signer)
        {
            message = DomainTag.AddUserDomainTag(message);
            return signer.Sign(message);
        }
    }
}
