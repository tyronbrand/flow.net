namespace Flow.Net.Sdk.Core.Crypto
{
    public interface ISigner
    {
        /// <summary>
        /// Signs the given message.
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns>A signed message as <see cref="byte"/>[]</returns>
        byte[] Sign(byte[] bytes);
    }
}
