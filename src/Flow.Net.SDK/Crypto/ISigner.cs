namespace Flow.Net.Sdk.Crypto
{
    public interface ISigner
    {
        byte[] Sign(byte[] bytes);
    }
}
