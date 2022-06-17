namespace Flow.Net.Sdk.Cadence.Types
{
    public interface ICadenceTypeResponse<TResponse>
    {
        TResponse Type { get; set; }
    }
}
