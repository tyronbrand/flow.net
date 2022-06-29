namespace Flow.Net.Sdk.Core.Cadence.Types
{
    public interface ICadenceType
    {
        string Kind { get; }

        string Encode(ICadenceType cadenceType);
    }
}
