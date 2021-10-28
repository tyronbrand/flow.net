using Flow.Net.Sdk;
using Flow.Net.Sdk.Cadence;
using Flow.Net.Sdk.Client;
using Flow.Net.Sdk.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Flow.Net.Examples
{
    public class CreateTransactionExample
    {
        private static async Task Demo()
        {
            // reading script from folder
            var script = Sdk.Utilities.ReadCadenceScript("greeting");

            var proposerAddress = "9a0766d93b6608b7".FromHexToByteString();
            uint proposerKeyIndex = 3;

            var payerAddress = "631e88ae7f1d7c20".FromHexToByteString();
            var authorizerAddress = "7aad92e5a0715d21".FromHexToByteString();

            // Establish a connection with an access node
            var accessAPIHost = "";
            var _flowClient = FlowClientAsync.Create(accessAPIHost);

            // Get the latest sealed block to use as a reference block
            var latestBlock = await _flowClient.GetLatestBlockHeaderAsync();

            // Get the latest account info for this address
            var proposerAccount = await _flowClient.GetAccountAtLatestBlockAsync(proposerAddress);

            // Get the latest sequence number for this key
            var proposerKey = proposerAccount.Keys.Where(w => w.Index == proposerKeyIndex).FirstOrDefault();
            var sequenceNumber = proposerKey.SequenceNumber;

            var tx = new FlowTransaction
            {
                Script = script,
                GasLimit = 100,
                ProposalKey = new FlowProposalKey
                {
                    Address = proposerAddress,
                    KeyId = proposerKeyIndex,
                    SequenceNumber = sequenceNumber
                },
                Payer = payerAddress
            };

            // Add authorizer(s)
            tx.Authorizers.Add(authorizerAddress);

            // Add argument(s)
            var arguments = new List<ICadence>
            {
                new CadenceString("Hello")
            };
            tx.Arguments = arguments.ToTransactionArguments();

        }

        private static void Demo2()
        {
            var proposerAccount = new FlowAccount();
            var proposerKey = proposerAccount.Keys.Where(w => w.Index == 1).FirstOrDefault();

            var tx = new FlowTransaction
            {
                Script = "transaction { execute { log(\"Hello, World!\") } }",
                GasLimit = 100,
                ProposalKey = new FlowProposalKey
                {
                    Address = proposerAccount.Address,
                    KeyId = proposerKey.Index,
                    SequenceNumber = proposerKey.SequenceNumber
                },
                Payer = proposerAccount.Address
            };

            // construct a signer from your private key and configured signature/hash algorithms
            var signer = Sdk.Crypto.Ecdsa.Utilities.CreateSigner("privateKey", SignatureAlgo.ECDSA_P256, HashAlgo.SHA3_256);

            tx.AddEnvelopeSignature(proposerAccount.Address, proposerKey.Index, signer);
        }
    }
}
