using Flow.Net.Sdk.Extensions;
using Flow.Net.Sdk.Models;
using Flow.Net.Sdk.RecursiveLengthPrefix;
using Xunit;
using Xunit.Abstractions;

namespace Flow.Net.Sdk.Tests
{
    public class FlowTransactionTest
    {
        private readonly ITestOutputHelper output;

        public FlowTransactionTest(ITestOutputHelper output)
        {
            this.output = output;
        }

        public static FlowTransaction Transaction()
        {
            var transaction = new FlowTransaction
            {
                ProposalKey = new FlowProposalKey
                {
                    Address = "01".FromHexToByteString(),
                    KeyId = 4,
                    SequenceNumber = 10
                },
                GasLimit = 42,
                Payer = "01".FromHexToByteString(),
                ReferenceBlockId = "f0e4c2f76c58916ec258f246851bea091d14d4247a2fc3e18694461b1816e13b".FromHexToByteString(),
                Script = "transaction { execute { log(\"Hello, World!\") } }"
            };

            transaction.Authorizers.Add("01".FromHexToByteString());

            transaction.PayloadSignatures.Add(
                new FlowSignature
                {
                    Address = "01".FromHexToByteString(),
                    KeyId = 4,
                    Signature = "f7225388c1d69d57e6251c9fda50cbbf9e05131e5adb81e5aa0422402f048162".FromHexToBytes()
                });

            return transaction;
        }

        [Fact]
        public void TestCompleteTransaction()
        {
            var payload = "f872b07472616e73616374696f6e207b2065786563757465207b206c6f67282248656c6c6f2c20576f726c64212229207d207dc0a0f0e4c2f76c58916ec258f246851bea091d14d4247a2fc3e18694461b1816e13b2a880000000000000001040a880000000000000001c9880000000000000001";
            var envelope = "f899f872b07472616e73616374696f6e207b2065786563757465207b206c6f67282248656c6c6f2c20576f726c64212229207d207dc0a0f0e4c2f76c58916ec258f246851bea091d14d4247a2fc3e18694461b1816e13b2a880000000000000001040a880000000000000001c9880000000000000001e4e38004a0f7225388c1d69d57e6251c9fda50cbbf9e05131e5adb81e5aa0422402f048162";

            var transaction = Transaction();

            var payloadTest = Rlp.EncodedCanonicalPayload(transaction);
            Assert.Equal(payload, payloadTest.FromByteArrayToHex());

            var envelopeTest = Rlp.EncodedCanonicalAuthorizationEnvelope(transaction);
            Assert.Equal(envelope, envelopeTest.FromByteArrayToHex());
        }

        [Fact]
        public void TestCompleteTransactionWithEnvelopeSig()
        {
            var payload = "f872b07472616e73616374696f6e207b2065786563757465207b206c6f67282248656c6c6f2c20576f726c64212229207d207dc0a0f0e4c2f76c58916ec258f246851bea091d14d4247a2fc3e18694461b1816e13b2a880000000000000001040a880000000000000001c9880000000000000001";
            var envelope = "f899f872b07472616e73616374696f6e207b2065786563757465207b206c6f67282248656c6c6f2c20576f726c64212229207d207dc0a0f0e4c2f76c58916ec258f246851bea091d14d4247a2fc3e18694461b1816e13b2a880000000000000001040a880000000000000001c9880000000000000001e4e38004a0f7225388c1d69d57e6251c9fda50cbbf9e05131e5adb81e5aa0422402f048162";

            var transaction = Transaction();
            transaction.EnvelopeSignatures.Add(
                new FlowSignature
                {
                    Address = "01".FromHexToByteString(),
                    KeyId = 4,
                    Signature = "f7225388c1d69d57e6251c9fda50cbbf9e05131e5adb81e5aa0422402f048162".FromHexToBytes()
                });

            var payloadTest = Rlp.EncodedCanonicalPayload(transaction);
            Assert.Equal(payload, payloadTest.FromByteArrayToHex());

            var envelopeTest = Rlp.EncodedCanonicalAuthorizationEnvelope(transaction);
            Assert.Equal(envelope, envelopeTest.FromByteArrayToHex());
        }

        [Fact]
        public void TestMultipleAuthorizers()
        {
            var payload = "f87bb07472616e73616374696f6e207b2065786563757465207b206c6f67282248656c6c6f2c20576f726c64212229207d207dc0a0f0e4c2f76c58916ec258f246851bea091d14d4247a2fc3e18694461b1816e13b2a880000000000000001040a880000000000000001d2880000000000000001880000000000000002";
            var envelope = "f8a2f87bb07472616e73616374696f6e207b2065786563757465207b206c6f67282248656c6c6f2c20576f726c64212229207d207dc0a0f0e4c2f76c58916ec258f246851bea091d14d4247a2fc3e18694461b1816e13b2a880000000000000001040a880000000000000001d2880000000000000001880000000000000002e4e38004a0f7225388c1d69d57e6251c9fda50cbbf9e05131e5adb81e5aa0422402f048162";

            var transaction = Transaction();
            transaction.Authorizers.Add("02".FromHexToByteString());

            var payloadTest = Rlp.EncodedCanonicalPayload(transaction);
            Assert.Equal(payload, payloadTest.FromByteArrayToHex());

            var envelopeTest = Rlp.EncodedCanonicalAuthorizationEnvelope(transaction);
            Assert.Equal(envelope, envelopeTest.FromByteArrayToHex());
        }
    }
}
