using Flow.Net.Sdk.Core;
using Xunit;

namespace Flow.Net.Sdk.Tests
{
    public class DomainTagTest
    {
        [Fact]
        public void TestUserDomainTag()
        {
            const string expectedOutput = "464c4f572d56302e302d75736572000000000000000000000000000000000000";
            var userDomainTag = DomainTag.AddUserDomainTag(System.Array.Empty<byte>());
            Assert.Equal(expectedOutput, userDomainTag.BytesToHex());
        }

        [Fact]
        public void TestTransactionDomainTag()
        {
            const string expectedOutput = "464c4f572d56302e302d7472616e73616374696f6e0000000000000000000000";
            var transactionDomainTag = DomainTag.AddTransactionDomainTag(System.Array.Empty<byte>());
            Assert.Equal(expectedOutput, transactionDomainTag.BytesToHex());
        }
    }
}
