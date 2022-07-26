using Flow.Net.Sdk.Core;
using Flow.Net.Sdk.Core.Cadence;
using Flow.Net.Sdk.Core.Client;
using Flow.Net.Sdk.Core.Models;
using Flow.Net.Sdk.Core.Templates;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace Flow.Net.Examples
{
    public abstract class ExampleBase
    {
        protected static IFlowClient FlowClient { get; set; }

        protected static async Task<FlowAccount> CreateAccountAsync(IList<FlowAccountKey> newFlowAccountKeys, IList<FlowContract> flowContracts = null)
        {
            var response = await CreateAddressTransaction(FlowClient, newFlowAccountKeys, flowContracts);

            var newAccountAddress = response.Events.AccountCreatedAddress();

            // get new account details
            var newAccount = await FlowClient.GetAccountAtLatestBlockAsync(newAccountAddress.Address);
            newAccount.Keys = FlowAccountKey.UpdateFlowAccountKeys(newFlowAccountKeys, newAccount.Keys);
            return newAccount;
        }

        protected static async Task<string> RandomTransactionAsync()
        {
            // read flow.json
            var config = Utilities.ReadConfig();
            // get account from config
            var accountConfig = config.Accounts["emulator-account"];
            // get service account at latest block
            var serviceAccount = await FlowClient.GetAccountAtLatestBlockAsync(accountConfig.Address);
            // we can create a Signer with the serviceAccount and the accountConfig
            serviceAccount = Utilities.AddSignerFromConfigAccount(accountConfig, serviceAccount);

            // service key to use
            var serviceAccountKey = serviceAccount.Keys.FirstOrDefault();

            var latestBlock = await FlowClient.GetLatestBlockAsync();
            var tx = new FlowTransaction
            {
                Script = "transaction {}",
                ReferenceBlockId = latestBlock.Header.Id,
                Payer = serviceAccount.Address,
                ProposalKey = new FlowProposalKey
                {
                    Address = serviceAccount.Address,
                    KeyId = serviceAccountKey.Index,
                    SequenceNumber = serviceAccountKey.SequenceNumber
                }
            };

            tx = FlowTransaction.AddEnvelopeSignature(tx, serviceAccount.Address, serviceAccountKey.Index, serviceAccountKey.Signer);

            var response = await FlowClient.SendTransactionAsync(tx);
            return response.Id;
        }

        protected static async Task<FlowAddress> GenerateRandomAddressAsync()
        {
            // generate random key
            var flowAccountKey = FlowAccountKey.GenerateRandomEcdsaKey(SignatureAlgo.ECDSA_P256, HashAlgo.SHA3_256);

            var response = await CreateAddressTransaction(FlowClient, new List<FlowAccountKey> { flowAccountKey }, null);
            return response.Events.AccountCreatedAddress();
        }

        private static async Task<FlowTransactionResult> CreateAddressTransaction(IFlowClient flowClient, IEnumerable<FlowAccountKey> newFlowAccountKeys, IEnumerable<FlowContract> flowContracts)
        {
            if (flowClient != null)
                FlowClient = flowClient;

            // read flow.json
            var config = Utilities.ReadConfig();
            // get account from config
            var accountConfig = config.Accounts["emulator-account"];
            // get service account at latest block
            var serviceAccount = await FlowClient.GetAccountAtLatestBlockAsync(accountConfig.Address);
            // add a Signer with the serviceAccount and the accountConfig
            serviceAccount = Utilities.AddSignerFromConfigAccount(accountConfig, serviceAccount);

            // creator key to use
            var serviceAccountKey = serviceAccount.Keys.FirstOrDefault();

            // use template to create a transaction
            var tx = AccountTemplates.CreateAccount(newFlowAccountKeys, serviceAccount.Address, flowContracts);

            // set the transaction payer and proposal key
            tx.Payer = serviceAccount.Address;
            tx.ProposalKey = new FlowProposalKey
            {
                Address = serviceAccount.Address,
                KeyId = serviceAccountKey.Index,
                SequenceNumber = serviceAccountKey.SequenceNumber
            };

            // get the latest sealed block to use as a reference block
            var latestBlock = await FlowClient.GetLatestBlockAsync();
            tx.ReferenceBlockId = latestBlock.Header.Id;

            // sign and submit the transaction
            tx = FlowTransaction.AddEnvelopeSignature(tx, serviceAccount.Address, serviceAccountKey.Index, serviceAccountKey.Signer);

            var response = await FlowClient.SendTransactionAsync(tx);

            // wait for seal
            var sealedResponse = await FlowClient.WaitForSealAsync(response.Id);

            if (sealedResponse.Status != TransactionStatus.Sealed)
                return null;

            return sealedResponse;
        }

        protected static async Task FundAccountInEmulator(FlowAddress recipientAddress, decimal amount)
        {
            // read flow.json
            var config = Utilities.ReadConfig();
            // get account from config
            var accountConfig = config.Accounts["emulator-account"];
            // get service account at latest block
            var serviceAccount = await FlowClient.GetAccountAtLatestBlockAsync(accountConfig.Address);
            // add a Signer with the serviceAccount and the accountConfig
            serviceAccount = Utilities.AddSignerFromConfigAccount(accountConfig, serviceAccount);
            // service key to use
            var serviceAccountKey = serviceAccount.Keys.FirstOrDefault();

            // read contract address
            var fungibleTokenAddress = config.Contracts["FungibleToken"];
            var flowTokenAddress = config.Contracts["FlowToken"];

            // cadence arguments
            var recipient = new CadenceAddress(recipientAddress.Address);
            var cadenceAmount = new CadenceNumber(CadenceNumberType.UFix64, amount.ToString("F8", CultureInfo.InvariantCulture));

            var script = $@"
import FungibleToken from 0x{fungibleTokenAddress}
import FlowToken from 0x{flowTokenAddress}
transaction(recipient: Address, amount: UFix64) {{
	let tokenAdmin: &FlowToken.Administrator
	let tokenReceiver: &{{FungibleToken.Receiver}}
	prepare(signer: AuthAccount) {{
		self.tokenAdmin = signer
			.borrow<&FlowToken.Administrator>(from: /storage/flowTokenAdmin)
			?? panic(""Signer is not the token admin"")
		self.tokenReceiver = getAccount(recipient)
			.getCapability(/public/flowTokenReceiver)
			.borrow<&{{FungibleToken.Receiver}}>()
			?? panic(""Unable to borrow receiver reference"")
	}}
	execute {{
		let minter <- self.tokenAdmin.createNewMinter(allowedAmount: amount)
		let mintedVault <- minter.mintTokens(amount: amount)
		self.tokenReceiver.deposit(from: <-mintedVault)
		destroy minter
	}}
}}";
            var latestBlock = await FlowClient.GetLatestBlockHeaderAsync();
            var tx = new FlowTransaction
            {
                Script = script,
                Arguments = new List<ICadence>
                {
                   recipient,
                   cadenceAmount
                },
                ReferenceBlockId = latestBlock.Id,
                Payer = serviceAccount.Address,
                ProposalKey = new FlowProposalKey
                {
                    Address = serviceAccount.Address,
                    KeyId = serviceAccountKey.Index,
                    SequenceNumber = serviceAccountKey.SequenceNumber
                }
            };    

            tx.Authorizers.Add(serviceAccount.Address);

            tx = FlowTransaction.AddEnvelopeSignature(tx, serviceAccount.Address, serviceAccountKey.Index, serviceAccountKey.Signer);

            var response = await FlowClient.SendTransactionAsync(tx);
            var sealedResponse = await FlowClient.WaitForSealAsync(response.Id);

            if (sealedResponse.Status != TransactionStatus.Sealed)
                throw new Exception("Failed to fund account in emulator.");
        }
    }
}

