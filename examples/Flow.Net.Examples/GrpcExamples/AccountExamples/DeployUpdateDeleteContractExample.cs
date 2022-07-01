using Flow.Net.Sdk;
using Flow.Net.Sdk.Models;
using Flow.Net.Sdk.Templates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Flow.Net.Examples.GrpcExamples.AccountExamples
{
    public class DeployUpdateDeleteContractExample : GrpcExampleBase
    {
        public static async Task RunAsync()
        {
            Console.WriteLine("\nRunning DeployUpdateDeleteContractExample\n");
            await CreateFlowClientAsync();
            await Demo();
            Console.WriteLine("\nDeployUpdateDeleteContractExample Complete\n");
        }

        private static async Task Demo()
        {
            // generate our new account key
            var newFlowAccountKey = FlowAccountKey.GenerateRandomEcdsaKey(SignatureAlgo.ECDSA_P256, HashAlgo.SHA3_256);

            // create account
            var newAccountAddress = await CreateAccountAsync(new List<FlowAccountKey> { newFlowAccountKey });

            // deploy contract
            await DeployContractAsync(newFlowAccountKey, newAccountAddress.Address);

            // update contract
            await UpdateContractAsync(newFlowAccountKey, newAccountAddress.Address);

            // delete contract
            await DeleteContractAsync(newFlowAccountKey, newAccountAddress.Address);
        }

        private static async Task DeployContractAsync(FlowAccountKey newFlowAccountKey, FlowAddress newAccountAddress)
        {
            // get new account details
            var newAccount = await FlowClient.GetAccountAtLatestBlockAsync(newAccountAddress);

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
            var latestBlock = await FlowClient.GetLatestBlockAsync();
            tx.ReferenceBlockId = latestBlock.Id;

            // sign and submit the transaction
            var newAccountSigner = new Sdk.Crypto.Ecdsa.Signer(newFlowAccountKey.PrivateKey, newAccountKey.HashAlgorithm, newAccountKey.SignatureAlgorithm);
            tx = FlowTransaction.AddEnvelopeSignature(tx, newAccount.Address, newAccountKey.Index, newAccountSigner);

            var response = await FlowClient.SendTransactionAsync(tx);
            var sealedResponse = await FlowClient.WaitForSealAsync(response);

            if (sealedResponse.Status == Sdk.Protos.entities.TransactionStatus.Sealed)
            {
                Console.WriteLine($"Contract \"{flowContract.Name}\" deployed!");
            }
        }

        private static async Task UpdateContractAsync(FlowAccountKey newFlowAccountKey, FlowAddress newAccountAddress)
        {
            // get new account details
            var newAccount = await FlowClient.GetAccountAtLatestBlockAsync(newAccountAddress);

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
            var latestBlock = await FlowClient.GetLatestBlockAsync();
            tx.ReferenceBlockId = latestBlock.Id;

            // sign and submit the transaction
            var newAccountSigner = new Sdk.Crypto.Ecdsa.Signer(newFlowAccountKey.PrivateKey, newAccountKey.HashAlgorithm, newAccountKey.SignatureAlgorithm);
            tx = FlowTransaction.AddEnvelopeSignature(tx, newAccount.Address, newAccountKey.Index, newAccountSigner);

            var response = await FlowClient.SendTransactionAsync(tx);
            var sealedResponse = await FlowClient.WaitForSealAsync(response);

            if (sealedResponse.Status == Sdk.Protos.entities.TransactionStatus.Sealed)
            {
                Console.WriteLine($"Contract \"{flowContract.Name}\" updated!");
            }
        }

        private static async Task DeleteContractAsync(FlowAccountKey newFlowAccountKey, FlowAddress newAccountAddress)
        {
            // get new account details
            var newAccount = await FlowClient.GetAccountAtLatestBlockAsync(newAccountAddress);

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
            var latestBlock = await FlowClient.GetLatestBlockAsync();
            tx.ReferenceBlockId = latestBlock.Id;

            // sign and submit the transaction
            var newAccountSigner = new Sdk.Crypto.Ecdsa.Signer(newFlowAccountKey.PrivateKey, newAccountKey.HashAlgorithm, newAccountKey.SignatureAlgorithm);
            tx = FlowTransaction.AddEnvelopeSignature(tx, newAccount.Address, newAccountKey.Index, newAccountSigner);

            var response = await FlowClient.SendTransactionAsync(tx);
            var sealedResponse = await FlowClient.WaitForSealAsync(response);

            if (sealedResponse.Status == Sdk.Protos.entities.TransactionStatus.Sealed)
            {
                Console.WriteLine($"Contract \"{flowContractName}\" deleted!");
            }            
        }
    }
}
