## Generate Account and return FlowAccount

In order to keep these examples simple, we are going to use the method below to create an account and return a FlowAccount.

```csharp
public static async Task<FlowAccount> CreateAccountAsync(List<FlowAccountKey> newFlowAccountKeys)
{
    var _flowClient = FlowClientAsync.Create(networkUrl);

    // creator (typically a service account)
    var creatorAccount = await _flowClient.ReadAccountFromConfigAsync("emulator-account");

    // use template to create a transaction
    var tx = Account.CreateAccount(newFlowAccountKeys, creatorAccount.Address);

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

        // get new account deatils
        var newAccount = await _flowClient.GetAccountAtLatestBlockAsync(newAccountAddress.FromHexToByteString());
        newAccount.Keys = FlowAccountKey.UpdateFlowAccountKeys(newFlowAccountKeys, newAccount.Keys);
        return newAccount;
    }

    return null;
}
```

## Single party, single signature

```csharp
var _flowClient = FlowClientAsync.Create(networkUrl);

// generate a new key and account
var flowAccountKey = FlowAccountKey.NewEcdsaAccountKey(SignatureAlgo.ECDSA_P256, HashAlgo.SHA3_256, 1000);
var account1 = await CreateAccountAsync(new List<FlowAccountKey> { flowAccountKey });
var account1Key = account1.Keys.FirstOrDefault();

var lastestBlock = await _flowClient.GetLatestBlockAsync();
var tx = new FlowTransaction
{
    Script = "transaction {prepare(signer: AuthAccount) { log(signer.address) }}",
    GasLimit = 9999,
    Payer = account1.Address,
    ProposalKey = new FlowProposalKey
    {
        Address = account1.Address,
        KeyId = account1Key.Index,
        SequenceNumber = account1Key.SequenceNumber
    },
    ReferenceBlockId = lastestBlock.Id
};

// authorizers
tx.Authorizers.Add(account1.Address);

// account 1 signs the envelope with key 1
tx.AddEnvelopeSignature(account1.Address, account1Key.Index, account1Key.Signer);

// send transaction
await _flowClient.SendTransactionAsync(tx);
```

## Single party, multi signature

```csharp
var flowAccountKey1 = FlowAccountKey.NewEcdsaAccountKey(SignatureAlgo.ECDSA_P256, HashAlgo.SHA3_256, 500);
var flowAccountKey2 = FlowAccountKey.NewEcdsaAccountKey(SignatureAlgo.ECDSA_P256, HashAlgo.SHA3_256, 500);
var account1 = await CreateAccountAsync(new List<FlowAccountKey> { flowAccountKey1, flowAccountKey2 });
var account1Key1 = account1.Keys[0];
var account1Key2 = account1.Keys[1];

var lastestBlock = await _flowClient.GetLatestBlockAsync();
var tx = new FlowTransaction
{
    Script = "transaction {prepare(signer: AuthAccount) { log(signer.address) }}",
    GasLimit = 9999,
    Payer = account1.Address,
    ProposalKey = new FlowProposalKey
    {
        Address = account1.Address,
        KeyId = account1Key1.Index,
        SequenceNumber = account1Key1.SequenceNumber
    },
    ReferenceBlockId = lastestBlock.Id
};

// authorizers
tx.Authorizers.Add(account1.Address);

// account 1 signs the envelope with key 1
tx.AddEnvelopeSignature(account1.Address, account1Key1.Index, account1Key1.Signer);

// account 1 signs the envelope with key 2
tx.AddEnvelopeSignature(account1.Address, account1Key2.Index, account1Key2.Signer);
            
// send transaction
await _flowClient.SendTransactionAsync(tx);
```

## Multi party, single signature

```csharp
 var flowAccountKey1 = FlowAccountKey.NewEcdsaAccountKey(SignatureAlgo.ECDSA_P256,HashAlgo.SHA3_256, 1000);
var account1 = await CreateAccountAsync(new List<FlowAccountKey> { flowAccountKey1 });
var flowAccountKey2 = FlowAccountKey.NewEcdsaAccountKey(SignatureAlgo.ECDSA_P256, HashAlgo.SHA3_256, 1000);
var account2 = await CreateAccountAsync(new List<FlowAccountKey> { flowAccountKey2 });

var account1Key = account1.Keys.FirstOrDefault();
var account2Key = account2.Keys.FirstOrDefault();

var lastestBlock = await _flowClient.GetLatestBlockAsync();
var tx = new FlowTransaction
{
    Script = "transaction {prepare(signer: AuthAccount) { log(signer.address) }}",
    GasLimit = 9999,
    Payer = account2.Address,
    ProposalKey = new FlowProposalKey
    {
        Address = account1.Address,
        KeyId = account1Key.Index,
        SequenceNumber = account1Key.SequenceNumber
    },
    ReferenceBlockId = lastestBlock.Id
};

// authorizers
tx.Authorizers.Add(account1.Address);

// account 1 signs the payload with key 1
tx.AddPayloadSignature(account1.Address, account1Key.Index, account1Key.Signer);

// account 2 signs the envelope
tx.AddEnvelopeSignature(account2.Address, account2Key.Index, account2Key.Signer);

// send transaction
await _flowClient.SendTransactionAsync(tx);
```

## Multi party, multi signature

```csharp
var flowAccount1Key1 = FlowAccountKey.NewEcdsaAccountKey(SignatureAlgo.ECDSA_P256, HashAlgo.SHA2_256, 500);
var flowAccount1Key2 = FlowAccountKey.NewEcdsaAccountKey(SignatureAlgo.ECDSA_P256, HashAlgo.SHA2_256, 500);
var account1 = await CreateAccountAsync(new List<FlowAccountKey> { flowAccount1Key1, flowAccount1Key2 });

var flowAccount2Key3 = FlowAccountKey.NewEcdsaAccountKey(SignatureAlgo.ECDSA_P256, HashAlgo.SHA3_256, 500);
var flowAccount2Key4 = FlowAccountKey.NewEcdsaAccountKey(SignatureAlgo.ECDSA_P256, HashAlgo.SHA3_256, 500);
var account2 = await CreateAccountAsync(new List<FlowAccountKey> { flowAccount2Key3, flowAccount2Key4 });

var lastestBlock = await _flowClient.GetLatestBlockAsync();
var tx = new FlowTransaction
{
    Script = "transaction {prepare(signer: AuthAccount) { log(signer.address) }}",
    GasLimit = 9999,
    Payer = account2.Address,
    ProposalKey = new FlowProposalKey
    {
        Address = account1.Address,
        KeyId = account1.Keys[0].Index,
        SequenceNumber = account1.Keys[0].SequenceNumber
    },
    ReferenceBlockId = lastestBlock.Id
};

// authorizers
tx.Authorizers.Add(account1.Address);

// account 1 signs the payload with key 1
tx.AddPayloadSignature(account1.Address, account1.Keys[0].Index, account1.Keys[0].Signer);

// account 1 signs the payload with key 2
tx.AddPayloadSignature(account1.Address, account1.Keys[1].Index, account1.Keys[1].Signer);

// account 2 signs the envelope with key 3
tx.AddEnvelopeSignature(account2.Address, account2.Keys[0].Index, account2.Keys[0].Signer);

// account 2 signs the envelope with key 3
tx.AddEnvelopeSignature(account2.Address, account2.Keys[1].Index, account2.Keys[1].Signer);

// send transaction
await _flowClient.SendTransactionAsync(tx);
```

## Multi party, two authorizers

```csharp
var flowAccountKey1 = FlowAccountKey.NewEcdsaAccountKey(SignatureAlgo.ECDSA_P256, HashAlgo.SHA3_256, 1000);
var account1 = await CreateAccountAsync(new List<FlowAccountKey> { flowAccountKey1 });
var flowAccountKey2 = FlowAccountKey.NewEcdsaAccountKey(SignatureAlgo.ECDSA_P256, HashAlgo.SHA3_256, 1000);
var account2 = await CreateAccountAsync(new List<FlowAccountKey> { flowAccountKey2 });

var account1Key = account1.Keys.FirstOrDefault();
var account2Key = account2.Keys.FirstOrDefault();

var lastestBlock = await _flowClient.GetLatestBlockAsync();
var tx = new FlowTransaction
{
    Script = @"
transaction { 
    prepare(signer1: AuthAccount, signer2: AuthAccount) { 
        log(signer1.address)
        log(signer2.address)
    }
}",
    GasLimit = 9999,
    Payer = account2.Address,
    ProposalKey = new FlowProposalKey
    {
        Address = account1.Address,
        KeyId = account1Key.Index,
        SequenceNumber = account1Key.SequenceNumber
    },
    ReferenceBlockId = lastestBlock.Id
};

// authorizers
tx.Authorizers.Add(account1.Address);
tx.Authorizers.Add(account2.Address);

// account 1 signs the payload with key 1
tx.AddPayloadSignature(account1.Address, account1Key.Index, account1Key.Signer);

// account 2 signs the envelope
tx.AddEnvelopeSignature(account2.Address, account2Key.Index, account2Key.Signer);

// send transaction
await _flowClient.SendTransactionAsync(tx);
```

## Example

An example can be found [here](https://github.com/tyronbrand/flow.net/blob/main/examples/Flow.Net.Examples/TransactionExamples.cs).