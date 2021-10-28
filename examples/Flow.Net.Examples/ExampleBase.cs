using Flow.Net.Sdk;
using Flow.Net.Sdk.Cadence;
using Flow.Net.Sdk.Client;
using Flow.Net.Sdk.Models;
using Flow.Net.Sdk.Templates;
using Google.Protobuf;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Flow.Net.Examples
{
    public class ExampleBase
    {
        public static FlowClientAsync _flowClient;

        public static async Task<FlowClientAsync> CreateFlowClientAsync()
        {
            var networkUrl = "127.0.0.1:3569"; // emulator

            if(_flowClient == null)
            {
                _flowClient = FlowClientAsync.Create(networkUrl);
                await _flowClient.PingAsync();
            }

            return _flowClient;
        }

        public static async Task<FlowAccount> CreateAccountAsync(List<FlowAccountKey> newFlowAccountKeys)
        {
            await CreateFlowClientAsync();            
            
            // creator (typically a service account)
            var creatorAccount = await _flowClient.ReadAccountFromConfigAsync("emulator-account");
            // creator key to use
            var creatorAccountKey = creatorAccount.Keys.FirstOrDefault();

            // use template to create a transaction
            var tx = Account.CreateAccount(newFlowAccountKeys, creatorAccount.Address);

            // set the transaction payer and proposal key
            tx.Payer = creatorAccount.Address;
            tx.ProposalKey = new FlowProposalKey
            {
                Address = creatorAccount.Address,
                KeyId = creatorAccountKey.Index,
                SequenceNumber = creatorAccountKey.SequenceNumber
            };

            // get the latest sealed block to use as a reference block
            var latestBlock = await _flowClient.GetLatestBlockAsync();
            tx.ReferenceBlockId = latestBlock.Id;

            // sign and submit the transaction
            tx.AddEnvelopeSignature(creatorAccount.Address, creatorAccountKey.Index, creatorAccountKey.Signer);
            
            var response = await _flowClient.SendTransactionAsync(tx);

            // wait for seal
            var sealedResponse = await _flowClient.WaitForSealAsync(response);

            if (sealedResponse.Status == Sdk.Protos.entities.TransactionStatus.Sealed)
            {
                var newAccountAddress = sealedResponse.Events.AccountCreatedAddress();                

                // get new account deatils
                var newAccount = await _flowClient.GetAccountAtLatestBlockAsync(newAccountAddress.FromHexToByteString());
                newAccount.Keys = FlowAccountKey.UpdateFlowAccountKeys(newFlowAccountKeys, newAccount.Keys);
                return newAccount;
            }

            return null;
        }

        public static async Task<ByteString> RandomTransactionAsync()
        {
            // creator (typically a service account)
            var serviceAccount = await _flowClient.ReadAccountFromConfigAsync("emulator-account");
            // creator key to use
            var serviceAccountKey = serviceAccount.Keys.FirstOrDefault();

            var latestBlock = await _flowClient.GetLatestBlockAsync();
            var tx = new FlowTransaction
            {
                Script = "transaction {}",
                ReferenceBlockId = latestBlock.Id,
                Payer = serviceAccount.Address,
                ProposalKey = new FlowProposalKey
                {
                    Address = serviceAccount.Address,
                    KeyId = serviceAccountKey.Index,
                    SequenceNumber = serviceAccountKey.SequenceNumber
                }
            };

            tx.AddEnvelopeSignature(serviceAccount.Address, serviceAccountKey.Index, serviceAccountKey.Signer);

            var response = await _flowClient.SendTransactionAsync(tx);
            return response.Id;
        }
    }
}

