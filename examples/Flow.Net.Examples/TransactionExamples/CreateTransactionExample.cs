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
            var script = Utilities.ReadCadenceScript("greeting");

            var proposerAddress = new FlowAddress("9a0766d93b6608b7");
            uint proposerKeyIndex = 3;

            var payerAddress = new FlowAddress("631e88ae7f1d7c20");
            var authorizerAddress = new FlowAddress("7aad92e5a0715d21");

            // Establish a connection with an access node
            var accessAPIHost = "";
            var FlowClient = new FlowClientAsync(accessAPIHost);

            // Get the latest sealed block to use as a reference block
            var latestBlock = await FlowClient.GetLatestBlockHeaderAsync();

            // Get the latest account info for this address
            var proposerAccount = await FlowClient.GetAccountAtLatestBlockAsync(proposerAddress);

            // Get the latest sequence number for this key
            var proposerKey = proposerAccount.Keys.Where(w => w.Index == proposerKeyIndex).FirstOrDefault();
            var sequenceNumber = proposerKey.SequenceNumber;

            var tx = new FlowTransaction
            {
                Script = script,
                GasLimit = 100,
                ProposalKey = new FlowProposalKey
                {
                    Address = proposerAddress.Value,
                    KeyId = proposerKeyIndex,
                    SequenceNumber = sequenceNumber
                },
                Payer = payerAddress.Value
            };

            // Add authorizer(s)
            tx.Authorizers.Add(authorizerAddress.Value);

            // Add argument(s)
            var arguments = new List<ICadence>
            {
                new CadenceString("Hello")
            };
            tx.Arguments = arguments.GenerateTransactionArguments();
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
                    Address = proposerAccount.Address.Value,
                    KeyId = proposerKey.Index,
                    SequenceNumber = proposerKey.SequenceNumber
                },
                Payer = proposerAccount.Address.Value
            };

            // construct a signer from your private key and configured signature/hash algorithms
            var signer = Sdk.Crypto.Ecdsa.Utilities.CreateSigner("privateKey", SignatureAlgo.ECDSA_P256, HashAlgo.SHA3_256);

            tx.AddEnvelopeSignature(proposerAccount.Address, proposerKey.Index, signer);
        }
    }
}
