using Flow.Net.Sdk.Core;
using Flow.Net.Sdk.Core.Client;
using Flow.Net.Sdk.Core.Crypto;
using Flow.Net.Sdk.Core.Models;
using Flow.Net.Sdk.Core.Templates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flow.Net.Examples.StorageExamples
{
    public class StorageUsageExample : ExampleBase
    {
        public static async Task RunAsync(IFlowClient flowClient)
        {
            Console.WriteLine("\nRunning StorageUsageExample\n");
            FlowClient = flowClient;
            await Demo();
            Console.WriteLine("\nStorageUsageExample Complete\n");
        }

        private static async Task Demo()
        {
            // read flow.json
            var config = Utilities.ReadConfig();
            // get account from config
            var accountConfig = config.Accounts["emulator-account"];

            var demoAccount = await CreateDemoAccountWithLargeContract();

            await SendSaveLargeResourceTransaction(accountConfig, demoAccount);

            await FundAccountInEmulator(demoAccount.Address, 1);

            await SendSaveLargeResourceTransaction(accountConfig, demoAccount);
        }

        private static async Task<FlowAccount> CreateDemoAccountWithLargeContract()
        {
            // contract to deploy
            var contractSource = @"
pub contract StorageDemo {
    pub resource StorageTestResource {
	    pub let data: String
	    init(data: String) {
		    self.data = data
	    }
    }
    pub fun createStorageTestResource(_ data: String): @StorageTestResource {
	    return <- create StorageTestResource(data: data)
    }
}";
            var flowContract = new FlowContract
            {
                Name = "StorageDemo",
                Source = contractSource
            };

            // generate demo account
            var demoAccountKey = FlowAccountKey.GenerateRandomEcdsaKey(SignatureAlgo.ECDSA_P256, HashAlgo.SHA3_256);
            return await CreateAccountAsync(new List<FlowAccountKey> { demoAccountKey }, new List<FlowContract> { flowContract });
        }

        private static async Task SendSaveLargeResourceTransaction(FlowConfigAccount serviceAccountConfig, FlowAccount demoAccount)
        {
            // get service account at latest block
            var serviceAccount = await FlowClient.GetAccountAtLatestBlockAsync(serviceAccountConfig.Address);
            // add a Signer with the serviceAccount and the accountConfig
            serviceAccount = Utilities.AddSignerFromConfigAccount(serviceAccountConfig, serviceAccount);
            // service key
            var serviceAccountKey = serviceAccount.Keys.FirstOrDefault();

            // get demo account at latest block            
            var latestDemoAccount = await FlowClient.GetAccountAtLatestBlockAsync(demoAccount.Address.Address);
            // demo key
            var demoAccountKey = demoAccount.Keys.FirstOrDefault();
            if (latestDemoAccount.Keys.FirstOrDefault().SequenceNumber > demoAccountKey.SequenceNumber)
                demoAccountKey.SequenceNumber = latestDemoAccount.Keys.FirstOrDefault().SequenceNumber;
            
            var buffer = Encoding.UTF8.GetBytes("".PadRight(100000));
            var longString = Encoding.UTF8.GetString(buffer, 0, buffer.Length);

            var script = $@"
import StorageDemo from 0x{demoAccount.Address.Address}
transaction {{
	prepare(acct: AuthAccount) {{
		let storageUsed = acct.storageUsed
				
		// create resource and save it on the account 
		let bigResource <- StorageDemo.createStorageTestResource(""{longString}"")
		acct.save(<-bigResource, to: /storage/StorageDemo)
		let storageUsedAfter = acct.storageUsed
		if (storageUsed == storageUsedAfter) {{
			panic(""storage used will change"")
		}}
				
		if (storageUsedAfter > acct.storageCapacity) {{
			// this is where we could deposit more flow to acct to increase its storaga capacity if we wanted to
			log(""Storage used is over capacity. This transaction will fail if storage limits are on on this chain."")
        }}
	}}
}}";
            var latestBlock = await FlowClient.GetLatestBlockHeaderAsync();
            var tx = new FlowTransaction
            {
                Script = script,
                ReferenceBlockId = latestBlock.Id,
                Payer = serviceAccount.Address,
                ProposalKey = new FlowProposalKey
                {
                    Address = demoAccount.Address,
                    KeyId = demoAccountKey.Index,
                    SequenceNumber = demoAccountKey.SequenceNumber
                }
            };

            // add authorizer
            tx.Authorizers.Add(demoAccount.Address);

            //signatures            
            tx = FlowTransaction.AddPayloadSignature(tx, demoAccount.Address, demoAccountKey.Index, demoAccountKey.Signer);
            tx = FlowTransaction.AddEnvelopeSignature(tx, serviceAccount.Address, serviceAccountKey.Index, serviceAccountKey.Signer);
                        
            //send 
            var response = await FlowClient.SendTransactionAsync(tx);

            // wait for seal
            var sealedResponse = await FlowClient.WaitForSealAsync(response.Id);

            if(sealedResponse.Status == TransactionStatus.Sealed)
            {
                switch(sealedResponse.StatusCode)
                {
                    case 0:
                        Console.WriteLine("Successfully increased storage!");
                    break;
                    case 1:
                        Console.WriteLine(sealedResponse.ErrorMessage);
                    break;
                }
            }
        }
    }
}
