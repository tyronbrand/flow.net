using Flow.Net.Sdk.Cadence;
using Google.Protobuf;
using System.Collections.Generic;
using System.Linq;

namespace Flow.Net.Sdk.Models
{
    public abstract class FlowTransactionBase
    {
        public FlowTransactionBase()
        {
            Arguments = new List<ByteString>();
            Authorizers = new List<ByteString>();
            PayloadSignatures = new List<FlowSignature>();
            EnvelopeSignatures = new List<FlowSignature>();
            GasLimit = 9999;
        }

        public string Script { get; set; }
        public IList<ByteString> Arguments { get; set; }
        public ByteString ReferenceBlockId { get; set; }
        public ulong GasLimit { get; set; }
        public ByteString Payer { get; set; }
        public FlowProposalKey ProposalKey { get; set; }
        public IList<ByteString> Authorizers { get; set; }
        public IList<FlowSignature> PayloadSignatures { get; set; }
        public IList<FlowSignature> EnvelopeSignatures { get; set; }

        /// <summary>
        /// Encodes the elements of <see cref="IEnumerable{T}" /> for use in <see cref="FlowTransactionBase.Arguments"/>.
        /// </summary>
        /// <param name="cadenceValues"></param>
        /// <returns>Arguments JSON encoded as a <see cref="IList{T}" /> of <see cref="ByteString"/>.</returns>
        public static IList<ByteString> ToTransactionArguments(IEnumerable<ICadence> cadenceValues)
        {
            var arguments = new List<ByteString>();

            if (cadenceValues != null && cadenceValues.Count() > 0)
            {
                foreach (var value in cadenceValues)
                {
                    var serialized = value.Encode();
                    arguments.Add(serialized.FromStringToByteString());
                }
            }

            return arguments;
        }
    }
}
