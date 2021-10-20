using Flow.Net.Sdk;
using Flow.Net.Sdk.Extensions;
using Flow.Net.Sdk.Cadence;
using Flow.Net.Sdk.Client;
using Flow.Net.Sdk.Models;
using Flow.Net.Sdk.Templates;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Flow.Net.Examples
{
    public class DeployUpdateDeleteContractExample
    {
        public static readonly string networkUrl = "127.0.0.1:3569"; // emulator
        public static FlowClientAsync _flowClient;

        public static async Task RunAsync()
        {
            // create a flow client
            _flowClient = FlowClientAsync.Create(networkUrl);

            // generate our new account key
            var newFlowAccountKey = FlowAccountKey.NewEcdsaAccountKey(SignatureAlgo.ECDSA_P256, HashAlgo.SHA3_256, 1000);

            // create account
            var newAccountAddress = await CreateAccountAsync(newFlowAccountKey);

            // deploy contract
            await DeployContractAsync(newFlowAccountKey, newAccountAddress);

            // update contract
            await UpdateContractAsync(newFlowAccountKey, newAccountAddress);

            // delete contract
            await DeleteContractAsync(newFlowAccountKey, newAccountAddress);
        }

        private static async Task DeployContractAsync(FlowAccountKey newFlowAccountKey, string newAccountAddress)
        {
            // get new account details
            var newAccount = await _flowClient.GetAccountAtLatestBlockAsync(newAccountAddress.FromHexToByteString());                      

            // contract to deploy
            var helloWorldContract = Utilities.ReadCadenceScript("hello-world-contract");
            var flowContract = new FlowContract
            {
                Name = "HelloWorld",
                Source = helloWorldContract
            };

            // use template to create a transaction
            var tx = Account.AddAccountContract(flowContract, newAccount.Address);

            // key to use
            var newAccountKey = newAccount.Keys.FirstOrDefault();            

            // set the transaction payer and proposal key
            tx.Payer = newAccount.Address;
            tx.ProposalKey = new FlowProposalKey
            {
                Address = newAccount.Address,
                KeyId = newAccountKey.Index,
                SequenceNumber = newAccountKey.SequenceNumber
            };

            // get the latest sealed block to use as a reference block
            var latestBlock = await _flowClient.GetLatestBlockAsync();

            tx.ReferenceBlockId = latestBlock.Id;

            // sign and submit the transaction
            var newAccountSigner = new Sdk.Crypto.Ecdsa.Signer(newFlowAccountKey.PrivateKey, newAccountKey.HashAlgorithm, newAccountKey.SignatureAlgorithm);
            tx.AddEnvelopeSignature(newAccount.Address, newAccountKey.Index, newAccountSigner);

            var response = await _flowClient.SendTransactionAsync(tx);
            await _flowClient.WaitForSealAsync(response);
        }

        private static async Task UpdateContractAsync(FlowAccountKey newFlowAccountKey, string newAccountAddress)
        {
            // get new account deatils
            var newAccount = await _flowClient.GetAccountAtLatestBlockAsync(newAccountAddress.FromHexToByteString());

            // contract to update
            var helloWorldContract = Utilities.ReadCadenceScript("hello-world-updated-contract");
            var flowContract = new FlowContract
            {
                Name = "HelloWorld",
                Source = helloWorldContract
            };

            // use template to create a transaction
            var tx = Account.UpdateAccountContract(flowContract, newAccount.Address);

            // key to use
            var newAccountKey = newAccount.Keys.FirstOrDefault();

            // set the transaction payer and proposal key
            tx.Payer = newAccount.Address;
            tx.ProposalKey = new FlowProposalKey
            {
                Address = newAccount.Address,
                KeyId = newAccountKey.Index,
                SequenceNumber = newAccountKey.SequenceNumber
            };

            // get the latest sealed block to use as a reference block
            var latestBlock = await _flowClient.GetLatestBlockAsync();

            tx.ReferenceBlockId = latestBlock.Id;

            // sign and submit the transaction
            var newAccountSigner = new Sdk.Crypto.Ecdsa.Signer(newFlowAccountKey.PrivateKey, newAccountKey.HashAlgorithm, newAccountKey.SignatureAlgorithm);
            tx.AddEnvelopeSignature(newAccount.Address, newAccountKey.Index, newAccountSigner);

            var response = await _flowClient.SendTransactionAsync(tx);
            await _flowClient.WaitForSealAsync(response);
        }

        private static async Task DeleteContractAsync(FlowAccountKey newFlowAccountKey, string newAccountAddress)
        {
            // get new account deatils
            var newAccount = await _flowClient.GetAccountAtLatestBlockAsync(newAccountAddress.FromHexToByteString());

            // contract to delete
            var flowContractName = "HelloWorld";

            // use template to create a transaction
            var tx = Account.DeleteAccountContract(flowContractName, newAccount.Address);

            // key to use
            var newAccountKey = newAccount.Keys.FirstOrDefault();

            // set the transaction payer and proposal key
            tx.Payer = newAccount.Address;
            tx.ProposalKey = new FlowProposalKey
            {
                Address = newAccount.Address,
                KeyId = newAccountKey.Index,
                SequenceNumber = newAccountKey.SequenceNumber
            };

            // get the latest sealed block to use as a reference block
            var latestBlock = await _flowClient.GetLatestBlockAsync();

            tx.ReferenceBlockId = latestBlock.Id;

            // sign and submit the transaction
            var newAccountSigner = new Sdk.Crypto.Ecdsa.Signer(newFlowAccountKey.PrivateKey, newAccountKey.HashAlgorithm, newAccountKey.SignatureAlgorithm);
            tx.AddEnvelopeSignature(newAccount.Address, newAccountKey.Index, newAccountSigner);

            var response = await _flowClient.SendTransactionAsync(tx);
            await _flowClient.WaitForSealAsync(response);
        }

        private static async Task<string> CreateAccountAsync(FlowAccountKey newFlowAccountKey)
        {
            // creator (typically a service account)
            var creatorAccount = await _flowClient.ReadAccountFromConfigAsync("emulator-account");

            // use template to create a transaction
            var tx = Account.CreateAccount(new List<FlowAccountKey> { newFlowAccountKey }, creatorAccount.Address);

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

            var response = await _flowClient.SendTransactionAsync(tx);

            // wait for seal
            var sealedResponse = await _flowClient.WaitForSealAsync(response);

            if (sealedResponse.Status == Sdk.Protos.entities.TransactionStatus.Sealed)
            {
                var newAccountAddress = sealedResponse.Events.AccountCreatedAddress();
                return newAccountAddress;
            }

            return null;                
        }
    }
}
