## Generating account keys

A Flow account can contain zero or more public keys, referred to as account keys.

An account key contains the following data:

- Raw public key
- Signature algorithm
- Hash algorithm
- Weight (integer between 0-1000)

```csharp
var flowAccountKey = FlowAccountKey.NewEcdsaAccountKey(SignatureAlgo.ECDSA_secp256k1, HashAlgo.SHA3_256, 1000);
```

## Construct the transaction

```csharp
// create a flow client
var _flowClient = FlowClientAsync.Create(networkUrl);

// creator (typically a service account)
var creatorAccount = await _flowClient.ReadAccountFromConfigAsync("emulator-account");

// generate our new account key
var flowAccountKey = FlowAccountKey.NewEcdsaAccountKey(SignatureAlgo.ECDSA_secp256k1, HashAlgo.SHA3_256, 1000);

// use template to create a transaction
var tx = Account.CreateAccount(new List<FlowAccountKey> { flowAccountKey }, creatorAccount.Address);                       

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
```

## Fetch the result

The new account address will be emitted in a system-level `flow.AccountCreated` event.

This event is returned as part of the transaction result:

```csharp
// wait for seal
var sealedResponse = await _flowClient.WaitForSealAsync(response);

if (sealedResponse.Status == Sdk.Protos.entities.TransactionStatus.Sealed)
{
    // the .AccountCreatedAddress() extension method will return the newly created account address
    var newAccountAddress = sealedResponse.Events.AccountCreatedAddress();
}
```

## Next steps

See how to deploy a contract to this account [here](https://github.com/tyronbrand/flow.net/blob/main/docs/deploy-contract.md).

## Example

An example can be found [here](https://github.com/tyronbrand/flow.net/blob/main/examples/Flow.Net.Examples/CreateAccountExample.cs).