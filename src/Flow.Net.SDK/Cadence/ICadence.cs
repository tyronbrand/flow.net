namespace Flow.Net.Sdk.Cadence
{
    public interface ICadence
    {
        string Type { get; }

        object Decode();
    }
}