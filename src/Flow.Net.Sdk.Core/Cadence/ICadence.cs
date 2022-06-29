namespace Flow.Net.Sdk.Core.Cadence
{
    public interface ICadence
    {
        string Type { get; }

        string Encode(ICadence cadence);
        //ICadence CompositeField(CadenceComposite cadenceComposite, string fieldName);
        //T CompositeFieldAs<T>(CadenceComposite cadenceComposite, string fieldName) where T : ICadence;
    }
}
