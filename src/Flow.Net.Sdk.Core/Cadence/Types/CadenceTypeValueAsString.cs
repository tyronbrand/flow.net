namespace Flow.Net.Sdk.Core.Cadence.Types
{
    public class CadenceTypeValueAsString : CadenceType
    {
        public CadenceTypeValueAsString() { }
        public CadenceTypeValueAsString(string value)
        {
            Value = value;
        }

        public override string Kind => "String Value";

        public string Value { get; set; }
    }
}
