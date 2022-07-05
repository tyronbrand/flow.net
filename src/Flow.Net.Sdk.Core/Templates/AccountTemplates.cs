using Flow.Net.Sdk.Core.Cadence;
using Flow.Net.Sdk.Core.Exceptions;
using Flow.Net.Sdk.Core.Models;
using System.Collections.Generic;
using System.Linq;

namespace Flow.Net.Sdk.Core.Templates
{
    public static class AccountTemplates
	{
		private const string CreateAccountTemplate = @"
transaction(publicKeys: [String], sigAlgos: [SignatureAlgorithm], hashAlgos: [HashAlgorithm], weights: [UFix64], contracts: {String: String}) {
	prepare(signer: AuthAccount) {
		let acct = AuthAccount(payer: signer)

		if (sigAlgos.length != publicKeys.length || hashAlgos.length != publicKeys.length || weights.length != publicKeys.length) {
			panic(""Length missmatch of passed arguments: public keys, signature algorithms, hashing algorithms and weights arrays must all have same length."")
		}

		for i, publicKey in publicKeys {
		    let key = PublicKey(
				publicKey: publicKey.decodeHex(),
				signatureAlgorithm: sigAlgos[i]
			)

			acct.keys.add(
				publicKey: key,
				hashAlgorithm: hashAlgos[i],
				weight: weights[i]
		    )
		}

		for contract in contracts.keys {
			acct.contracts.add(name: contract, code: contracts[contract]!.decodeHex())
		}
	}
}";


		public static FlowTransaction CreateAccount(IEnumerable<FlowAccountKey> flowAccountKeys, FlowAddress authorizerAddress, IEnumerable<FlowContract> flowContracts = null)
		{
			if (flowAccountKeys == null)
				throw new FlowException("Flow account key required.");

			flowAccountKeys = flowAccountKeys.ToList();

			if (!flowAccountKeys.Any())
				throw new FlowException("Flow account key required.");

			var publicKeys = new List<ICadence>();
			var sigAlgos = new List<ICadence>();
			var hashAlgos = new List<ICadence>();
			var weights = new List<ICadence>();

			foreach (var key in flowAccountKeys)
			{
				publicKeys.Add(new CadenceString(key.PublicKey));

				sigAlgos.Add(
					new CadenceComposite(
						CadenceCompositeType.Enum,
						new CadenceCompositeItem
						{
							Id = "SignatureAlgorithm",
							Fields = new List<CadenceCompositeItemValue>
							{
								new CadenceCompositeItemValue
								{
									Name = "rawValue",
									Value = new CadenceNumber(CadenceNumberType.UInt8, key.SignatureAlgorithm.FromSignatureAlgoToCadenceSignatureAlgorithm().GetHashCode().ToString())
								}
							}
						}
					)
				);

				hashAlgos.Add(
					new CadenceComposite(
						CadenceCompositeType.Enum,
						new CadenceCompositeItem
						{
							Id = "HashAlgorithm",
							Fields = new List<CadenceCompositeItemValue>
							{
								new CadenceCompositeItemValue
								{
									Name = "rawValue",
									Value = new CadenceNumber(CadenceNumberType.UInt8, key.HashAlgorithm.FromHashAlgoToCadenceHashAlgorithm().GetHashCode().ToString())
								}
							}
						}
					)
				);

				weights.Add(new CadenceNumber(CadenceNumberType.UFix64, $"{key.Weight}.0"));
			}

			var contracts = new CadenceDictionary();

			if (flowContracts != null)
			{
				flowContracts = flowContracts.ToList();

				if (flowContracts.Any())
				{
					foreach (var contract in flowContracts)
					{
						contracts.Value.Add(
							new CadenceDictionaryKeyValue
							{
								Key = new CadenceString(contract.Name),
								Value = new CadenceString(contract.Source.StringToHex())
							});
					}
				}
			}

			var tx = new FlowTransaction
			{
				Script = CreateAccountTemplate,
				Arguments = new List<ICadence>
				{
					new CadenceArray(publicKeys),
					new CadenceArray(sigAlgos),
					new CadenceArray(hashAlgos),
					new CadenceArray(weights),
					contracts
				}
			};

			// add authorizer
			tx.Authorizers.Add(authorizerAddress);

			return tx;
		}

		private static FlowTransaction AccountContractBase(string script, FlowContract flowContract, FlowAddress authorizerAddress)
		{
			var tx = new FlowTransaction
			{
				Script = script,
				Arguments = new List<ICadence>
				{
					new CadenceString(flowContract.Name),
					new CadenceString(flowContract.Source.StringToHex())
				}
			};

			// add authorizer
			tx.Authorizers.Add(authorizerAddress);

			return tx;
		}

		private const string AddAccountContractTemplate = @"
transaction(name: String, code: String)
{
	prepare(signer: AuthAccount) {
		signer.contracts.add(name: name, code: code.decodeHex())
	}
}";

		public static FlowTransaction AddAccountContract(FlowContract flowContract, FlowAddress authorizerAddress)
		{
			return AccountContractBase(AddAccountContractTemplate, flowContract, authorizerAddress);
		}

		private const string UpdateAccountContractTemplate = @"
transaction(name: String, code: String)
{
	prepare(signer: AuthAccount) {
		signer.contracts.update__experimental(name: name, code: code.decodeHex())
	}
}";

		public static FlowTransaction UpdateAccountContract(FlowContract flowContract, FlowAddress authorizerAddress)
		{
			return AccountContractBase(UpdateAccountContractTemplate, flowContract, authorizerAddress);
		}

		private const string DeleteAccountContractTemplate = @"
transaction(name: String)
{
	prepare(signer: AuthAccount) {
		signer.contracts.remove(name: name)
	}
}";

		public static FlowTransaction DeleteAccountContract(string contractName, FlowAddress authorizerAddress)
		{
			var tx = new FlowTransaction
			{
				Script = DeleteAccountContractTemplate
			};

			// add argument
			tx.Arguments.Add(new CadenceString(contractName));

			// add authorizer
			tx.Authorizers.Add(authorizerAddress);

			return tx;
		}
	}
}
