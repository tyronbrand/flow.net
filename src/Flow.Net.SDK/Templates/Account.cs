using Flow.Net.Sdk.Cadence;
using Flow.Net.Sdk.Exceptions;
using Flow.Net.Sdk.Models;
using Flow.Net.Sdk.RecursiveLengthPrefix;
using Google.Protobuf;
using System.Collections.Generic;
using System.Linq;

namespace Flow.Net.Sdk.Templates
{
    public class Account
    {
		private static readonly string CreateAccountTemplate = @"
transaction(publicKeys: [String], contracts: { String: String})
{
	prepare(signer: AuthAccount)
	{
		let acct = AuthAccount(payer: signer)
		for key in publicKeys {
				acct.addPublicKey(key.decodeHex())
		}
		for contract in contracts.keys {
			acct.contracts.add(name: contract, code: contracts[contract]!.decodeHex())
		}
	}
}";

        public static FlowTransaction CreateAccount(IEnumerable<FlowAccountKey> flowAccountKeys, ByteString authorizerAddress, IEnumerable<FlowContract> flowContracts = null)
        {
			if (flowAccountKeys == null || flowAccountKeys.Count() == 0)
				throw new FlowException("Flow account key required.");

			var accountKeys = new CadenceArray();
			foreach (var key in flowAccountKeys)
			{
				accountKeys.Value.Add(
					new CadenceString(
						Rlp.EncodedAccountKey(key).FromByteArrayToHex()
					));
			}

			var contracts = new CadenceDictionary();
			if (flowContracts != null && flowContracts.Count() > 0)
            {
				foreach(var contract in flowContracts)
                {
					contracts.Value.Add(
					new CadenceDictionaryKeyValue
					{
						Key = new CadenceString(contract.Name),
						Value = new CadenceString(contract.Source.FromStringToHex())
					});
				}				
            }

			var tx = new FlowTransaction
			{
				Script = CreateAccountTemplate
			};

			// add arguments
			tx.Arguments = 
				new List<ICadence>
				{
					accountKeys,
					contracts
				}.ToTransactionArguments();

			// add authorizer
			tx.Authorizers.Add(authorizerAddress);

			return tx;
        }

		private static FlowTransaction AccountContractBase(string script, FlowContract flowContract, ByteString authorizerAddress)
        {
			var tx = new FlowTransaction
			{
				Script = script
			};

			// add arguments
			tx.Arguments =
				new List<ICadence>
				{
					new CadenceString(flowContract.Name),
					new CadenceString(flowContract.Source.FromStringToHex())
				}.ToTransactionArguments();

			// add authorizer
			tx.Authorizers.Add(authorizerAddress);

			return tx;
		}

		private static readonly string AddAccountContractTemplate = @"
transaction(name: String, code: String)
{
	prepare(signer: AuthAccount) {
		signer.contracts.add(name: name, code: code.decodeHex())
	}
}";

		public static FlowTransaction AddAccountContract(FlowContract flowContract, ByteString authorizerAddress)
		{
			return AccountContractBase(AddAccountContractTemplate, flowContract, authorizerAddress);
		}

		private static readonly string UpdateAccountContractTemplate = @"
transaction(name: String, code: String)
{
	prepare(signer: AuthAccount) {
		signer.contracts.update__experimental(name: name, code: code.decodeHex())
	}
}";

		public static FlowTransaction UpdateAccountContract(FlowContract flowContract, ByteString authorizerAddress)
		{
			return AccountContractBase(UpdateAccountContractTemplate, flowContract, authorizerAddress);
		}

		private static readonly string DeleteAccountContractTemplate = @"
transaction(name: String)
{
	prepare(signer: AuthAccount) {
		signer.contracts.remove(name: name)
	}
}";
		public static FlowTransaction DeleteAccountContract(string contractName, ByteString authorizerAddress)
        {
			var tx = new FlowTransaction
			{
				Script = DeleteAccountContractTemplate
			};

			// add arguments
			tx.Arguments =
				new List<ICadence>
				{
					new CadenceString(contractName),
				}.ToTransactionArguments();

			// add authorizer
			tx.Authorizers.Add(authorizerAddress);

			return tx;
		}
	}
}
