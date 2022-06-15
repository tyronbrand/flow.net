namespace Flow.Net.Sdk.Cadence.Types
{
    public interface ICadenceType
    {
        string Kind { get; }

        string Encode(ICadenceType cadenceType);
    }
}
