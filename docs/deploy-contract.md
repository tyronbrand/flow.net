## Prerequisite

This example follows on from the create account example found [here](https://github.com/tyronbrand/flow.net/blob/main/docs/create-account.md).

## Deploying contract transaction

The contract that we will be deploying can be found [here](https://github.com/tyronbrand/flow.net/blob/main/examples/Flow.Net.Examples/Cadence/hello-world-contract.cdc).

```csharp
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

await _flowClient.SendTransactionAsync(tx);
```

## Next steps

See how to update this contract [here](https://github.com/tyronbrand/flow.net/blob/main/docs/update-contract.md).

## Example

An example can be found [here](https://github.com/tyronbrand/flow.net/blob/main/examples/Flow.Net.Examples/DeployUpdateDeleteContractExample.cs).
