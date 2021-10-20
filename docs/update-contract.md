## Prerequisite

This example follows on from the deploy contract example found [here](https://github.com/tyronbrand/flow.net/blob/main/docs/deploy-contract.md).

## Update contract transaction

The updated contract can be found [here](https://github.com/tyronbrand/flow.net/blob/main/examples/Flow.Net.Examples/Cadence/hello-world-updated-contract.cdc).

```csharp
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

await _flowClient.SendTransactionAsync(tx);
```

## Next steps

See how to remove this contract [here](https://github.com/tyronbrand/flow.net/blob/main/docs/remove-contract.md).

## Example

An example can be found [here](https://github.com/tyronbrand/flow.net/blob/main/examples/Flow.Net.Examples/DeployUpdateDeleteContractExample.cs).
