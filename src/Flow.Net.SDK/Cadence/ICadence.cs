using Google.Protobuf;
using System.Collections.Generic;

namespace Flow.Net.Sdk.Cadence
{
    public interface ICadence
    {
        string Type { get; }

        string Encode(ICadence cadence);
        ICadence Decode(string cadenceJson, CadenceConverter cadenceConverter = null);
        T As<T>(ICadence cadence) where T : ICadence;
        IList<ByteString> GenerateTransactionArguments(IEnumerable<ICadence> cadenceValues);
    }
}