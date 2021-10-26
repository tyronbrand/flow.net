using Flow.Net.Examples.Utilities;
using Flow.Net.Sdk;
using Flow.Net.Sdk.Cadence;
using Flow.Net.Sdk.Client;
using Flow.Net.Sdk.Models;
using Flow.Net.Sdk.Templates;
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
            
            foreach(var key in newFlowAccountKeys)
                ConvertToConsoleMessage.WriteInfoMessage(key);
            
            // creator (typically a service account)
            var creatorAccount = await _flowClient.ReadAccountFromConfigAsync("emulator-account");

            // use template to create a transaction
            var tx = Account.CreateAccount(newFlowAccountKeys, creatorAccount.Address);

            // creator key to use
            var creatorAccountKey = creatorAccount.Keys.FirstOrDefault();

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

            ConvertToConsoleMessage.WriteInfoMessage(tx);
            var response = await _flowClient.SendTransactionAsync(tx);

            // wait for seal
            var sealedResponse = await _flowClient.WaitForSealAsync(response);

            if (sealedResponse.Status == Sdk.Protos.entities.TransactionStatus.Sealed)
            {
                ConvertToConsoleMessage.WriteSuccessMessage(sealedResponse);
                var newAccountAddress = sealedResponse.Events.AccountCreatedAddress();
                ColorConsole.WriteSuccess($"\nnew account address: 0x{newAccountAddress}");

                // get new account deatils
                var newAccount = await _flowClient.GetAccountAtLatestBlockAsync(newAccountAddress.FromHexToByteString());
                newAccount.Keys = FlowAccountKey.UpdateFlowAccountKeys(newFlowAccountKeys, newAccount.Keys);
                return newAccount;
            }

            return null;
        }
    }
}
