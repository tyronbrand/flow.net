using Flow.Net.Sdk;
using Flow.Net.Sdk.Cadence;
using Flow.Net.Sdk.Client;
using Flow.Net.Sdk.EventType;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Flow.Net.Examples
{
    public class AccountExample
    {
        public static FlowClientAsync _flowClient;
        public static readonly string networkUrl = "127.0.0.1:3569"; // emulator
        //public static readonly string networkUrl = "access.devnet.nodes.onflow.org:9000"; // testnet

        public static void CreateClient()
        {
            _flowClient = FlowClientAsync.Create(networkUrl);
        }

        public static async Task RunAsync()
        {
            if(_flowClient == null)
                CreateClient();

            await _flowClient.PingAsync();
            
            // create account  
            var flowAccountKey = FlowAccountKey.NewEcdsaAccountKey(SignatureAlgo.ECDSA_secp256k1, HashAlgo.SHA3_256, 1000);
            var createAccountResult = await CreateAccount(new List<FlowAccountKey> { flowAccountKey });

            if (createAccountResult.Status == Sdk.Protos.entities.TransactionStatus.Sealed)
            {
                // get out new address
                var accountCreatedEventPayload = JsonConvert.SerializeObject(
                    createAccountResult.Events
                    .Where(w => w.Type == "flow.AccountCreated")
                    .FirstOrDefault()
                    .Payload);

                var accountAddress = JsonConvert.DeserializeObject<AccountCreated>(accountCreatedEventPayload).Value.Fields.FirstOrDefault().Value.Value.Remove0x();

                // deploy contract
                await DeployContract(flowAccountKey, accountAddress);

                // update contract
                await UpdateContract(flowAccountKey, accountAddress);

                // remove contract
                await RemoveContract(flowAccountKey, accountAddress);
            }         
        }

        public static async Task<FlowTransactionResult> CreateAccount(List<FlowAccountKey> flowAccountKeys)
        {
            if (_flowClient == null)
                CreateClient();

            // using our service account
            var serviceAccount = await _flowClient.ReadAccountFromConfigAsync("emulator-account");

            // selecting a key to use for the transaction
            var serviceAccountKey = serviceAccount.Keys.FirstOrDefault();

            var script = Utilities.ReadCadenceScript("create-account");
            var lastestBlock = await _flowClient.GetLatestBlockAsync();
            var tx = new FlowTransaction
            {
                Script = script,
                GasLimit = 9999,
                Payer = serviceAccount.Address,
                ProposalKey = new FlowProposalKey
                {
                    Address = serviceAccount.Address,
                    KeyId = serviceAccountKey.Index,
                    SequenceNumber = serviceAccountKey.SequenceNumber
                },
                ReferenceBlockId = lastestBlock.Id
            };

            // arguments
            var accountKeys = new List<ICadence>();
            foreach(var key in flowAccountKeys)
            {
                accountKeys.Add(
                    new CadenceString(
                        FlowAccountKey.RlpEncode(key).FromByteArrayToHex() // encoded publicKey param
                    ));
            }

            var arguments = new List<ICadence>
            {
                new CadenceArray(accountKeys),
                new CadenceDictionary() // contracts param (empty for now)
            };
            tx.Arguments = arguments.ToTransactionArguments();

            // authorizers
            tx.Authorizers.Add(serviceAccount.Address);

            // sign
            tx = FlowTransaction.AddEnvelopeSignature(tx, serviceAccount.Address, serviceAccountKey.Index, serviceAccountKey.Signer);

            // send transaction
            var txResponse = await _flowClient.SendTransactionAsync(tx);

            // wait for seal
            var sealedResponse = await _flowClient.WaitForSealAsync(txResponse);

            Console.WriteLine($"{JsonConvert.SerializeObject(sealedResponse)}\n");

            return sealedResponse;
        }

        public static async Task DeployContract(FlowAccountKey flowAccountKey, string accountAddress)
        {
            // get new account deatils
            var newAccount = await _flowClient.GetAccountAtLatestBlockAsync(accountAddress.FromHexToByteString());

            // update keys and generate signer (you could skip this and create your own signer if you wanted)
            newAccount.Keys = FlowAccountKey.UpdateFlowAccountKeys(new List<FlowAccountKey> { flowAccountKey }, newAccount.Keys);

            // selecting a key to use for the transaction
            var newAccountKey = newAccount.Keys.FirstOrDefault();

            var helloWorldContract = Utilities.ReadCadenceScript("hello-world-contract");
            var script = Utilities.ReadCadenceScript("add-contract");
            var lastestBlock = await _flowClient.GetLatestBlockAsync();
            var tx = new FlowTransaction
            {
                Script = script,
                GasLimit = 9999,
                Payer = newAccount.Address,
                ProposalKey = new FlowProposalKey
                {
                    Address = newAccount.Address,
                    KeyId = newAccountKey.Index,
                    SequenceNumber = newAccountKey.SequenceNumber
                },
                ReferenceBlockId = lastestBlock.Id
            };

            var arguments = new List<ICadence>
                {
                    new CadenceString("HelloWorld"),
                    new CadenceString(helloWorldContract.FromStringToHex())
                };
            tx.Arguments = arguments.ToTransactionArguments();

            // authorizers
            tx.Authorizers.Add(newAccount.Address);

            // sign
            tx = FlowTransaction.AddEnvelopeSignature(tx, newAccount.Address, newAccountKey.Index, newAccountKey.Signer);

            // send transaction
            var txResponse = await _flowClient.SendTransactionAsync(tx);

            // wait for seal
            var sealedResponse = await _flowClient.WaitForSealAsync(txResponse);

            Console.WriteLine($"{JsonConvert.SerializeObject(sealedResponse)}\n");

        }

        public static async Task UpdateContract(FlowAccountKey flowAccountKey, string accountAddress)
        {
            // get new account deatils
            var newAccount = await _flowClient.GetAccountAtLatestBlockAsync(accountAddress.FromHexToByteString());

            // update keys and generate signer (you could skip this and create your own signer if you wanted)
            newAccount.Keys = FlowAccountKey.UpdateFlowAccountKeys(new List<FlowAccountKey> { flowAccountKey }, newAccount.Keys);

            // selecting a key to use for the transaction
            var newAccountKey = newAccount.Keys.FirstOrDefault();

            var helloWorldContract = Utilities.ReadCadenceScript("hello-world-updated-contract");
            var script = Utilities.ReadCadenceScript("update-contract");
            var lastestBlock = await _flowClient.GetLatestBlockAsync();
            var tx = new FlowTransaction
            {
                Script = script,
                GasLimit = 9999,
                Payer = newAccount.Address,
                ProposalKey = new FlowProposalKey
                {
                    Address = newAccount.Address,
                    KeyId = newAccountKey.Index,
                    SequenceNumber = newAccountKey.SequenceNumber
                },
                ReferenceBlockId = lastestBlock.Id
            };

            var arguments = new List<ICadence>
                {
                    new CadenceString("HelloWorld"),
                    new CadenceString(helloWorldContract.FromStringToHex())
                };
            tx.Arguments = arguments.ToTransactionArguments();

            // authorizers
            tx.Authorizers.Add(newAccount.Address);

            // sign
            tx = FlowTransaction.AddEnvelopeSignature(tx, newAccount.Address, newAccountKey.Index, newAccountKey.Signer);

            // send transaction
            var txResponse = await _flowClient.SendTransactionAsync(tx);

            // wait for seal
            var sealedResponse = await _flowClient.WaitForSealAsync(txResponse);

            Console.WriteLine($"{JsonConvert.SerializeObject(sealedResponse)}\n");

        }

        public static async Task RemoveContract(FlowAccountKey flowAccountKey, string accountAddress)
        {
            // get new account deatils
            var newAccount = await _flowClient.GetAccountAtLatestBlockAsync(accountAddress.FromHexToByteString());

            // update keys and generate signer (you could skip this and create your own signer if you wanted)
            newAccount.Keys = FlowAccountKey.UpdateFlowAccountKeys(new List<FlowAccountKey> { flowAccountKey }, newAccount.Keys);

            // selecting a key to use for the transaction
            var newAccountKey = newAccount.Keys.FirstOrDefault();

            var script = Utilities.ReadCadenceScript("remove-contract");
            var lastestBlock = await _flowClient.GetLatestBlockAsync();
            var tx = new FlowTransaction
            {
                Script = script,
                GasLimit = 9999,
                Payer = newAccount.Address,
                ProposalKey = new FlowProposalKey
                {
                    Address = newAccount.Address,
                    KeyId = newAccountKey.Index,
                    SequenceNumber = newAccountKey.SequenceNumber
                },
                ReferenceBlockId = lastestBlock.Id
            };

            var arguments = new List<ICadence>
                {
                    new CadenceString("HelloWorld")
                };
            tx.Arguments = arguments.ToTransactionArguments();

            // authorizers
            tx.Authorizers.Add(newAccount.Address);

            // sign
            tx = FlowTransaction.AddEnvelopeSignature(tx, newAccount.Address, newAccountKey.Index, newAccountKey.Signer);

            // send transaction
            var txResponse = await _flowClient.SendTransactionAsync(tx);

            // wait for seal
            var sealedResponse = await _flowClient.WaitForSealAsync(txResponse);

            Console.WriteLine($"{JsonConvert.SerializeObject(sealedResponse)}\n");

        }
    }
}
