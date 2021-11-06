using Google.Protobuf;
using System.Collections.Generic;

namespace Flow.Net.Sdk.Cadence
{
    public interface ICadence
    {
        string Type { get; }

        string Encode(ICadence cadence);
        T As<T>(ICadence cadence) where T : ICadence;
        IList<ByteString> GenerateTransactionArguments(IEnumerable<ICadence> cadenceValues);
        ICadence CompositeField(CadenceComposite cadenceComposite, string fieldName);
        T CompositeFieldAs<T>(CadenceComposite cadenceComposite, string fieldName) where T : ICadence;
    }
}