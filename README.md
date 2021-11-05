<br />
<div align="center">
  <a href="#">
    <img src="https://tyronbrand.github.io/flow.net/flowdotnetfull.png" alt="Logo" width="500" height="auto">
  </a>
  <p align="center"> <br />
    <a href="https://github.com/tyronbrand/flow.net"><strong>View on GitHub »</strong></a> <br /><br />
    <a href="https://docs.onflow.org/sdk-guidelines/">SDK Specifications</a> ·
    <a href="https://github.com/tyronbrand/flow.net/blob/main/CONTRIBUTING.md">Contribute</a> ·
    <a href="https://github.com/tyronbrand/flow.net/issues">Report a Bug</a>
  </p>
</div><br />

## Overview 

This reference documents all the methods available in the Flow.Net SDK, and explains in detail how these methods work.

The library client specifications can be found here:

https://tyronbrand.github.io/flow.net/api

## Getting Started

### Installing

### [Package Manager Console](#tab/install-with-pmconsole)
Run following command in VS Package Manager Console:  
```
Install-Package Flow.Net.Sdk
```

### [Command Line](#tab/install-with-cli)
Run following command in command line:  
```
dotnet add package Flow.Net.Sdk
```
***

### Importing the Library
```
using Flow.Net.Sdk;
using Flow.Net.Sdk.Cadence;
using Flow.Net.Sdk.Client;
using Flow.Net.Sdk.Models;
using Flow.Net.Sdk.Templates;
using Flow.Net.Sdk.Constants;
using Flow.Net.Sdk.Converters;
```

## Connect
[<img src="https://raw.githubusercontent.com/onflow/sdks/main/templates/documentation/ref.svg" width="130">](https://tyronbrand.github.io/flow.net/api/Flow.Net.Sdk.Client.FlowClientAsync.html#Flow_Net_Sdk_Client_FlowClientAsync_Create_System_String_System_Boolean_System_Collections_Generic_List_Grpc_Core_ChannelOption__)

The library uses gRPC to communicate with the access nodes and it must be configured with correct access node API URL. 

📖 **Access API URLs** can be found [here](https://docs.onflow.org/access-api/#flow-access-node-endpoints). An error will be returned if the host is unreachable.
The Access Nodes APIs hosted by DapperLabs are accessible at:
- Testnet `access.devnet.nodes.onflow.org:9000`
- Mainnet `access.mainnet.nodes.onflow.org:9000`
- Local Emulator `127.0.0.1:3569` 

```csharp
var testnet = "access.devnet.nodes.onflow.org:9000";

var _flowClient = new FlowClientAsync(testnet);
```

## Querying the Flow Network
After you have established a connection with an access node, you can query the Flow network to retrieve data about blocks, accounts, events and transactions. We will explore how to retrieve each of these entities in the sections below.

### Get Blocks
[<img src="https://raw.githubusercontent.com/onflow/sdks/main/templates/documentation/ref.svg" width="130">](https://tyronbrand.github.io/flow.net/api/Flow.Net.Sdk.Client.FlowClientAsync.html#Flow_Net_Sdk_Client_FlowClientAsync_GetLatestBlockAsync_System_Boolean_Grpc_Core_CallOptions_)

Query the network for block by id, height or get the latest block.

📖 **Block ID** is SHA3-256 hash of the entire block payload. This hash is stored as an ID field on any block response object (ie. response from `GetLatestBlock`). 

📖 **Block height** expresses the height of the block on the chain. The latest block height increases by one for every valid block produced.

#### Examples

This example depicts ways to get the latest block as well as any other block by height or ID:

**[<img src="https://raw.githubusercontent.com/onflow/sdks/main/templates/documentation/try.svg" width="130">](https://github.com/tyronbrand/flow.net/tree/main/examples/Flow.Net.Examples/BlockExamples/BlockExample.cs)**
```csharp
private static async Task Demo()
{
    // get the latest sealed block
    var latestBlock = await _flowClient.GetLatestBlockAsync();
    PrintResult(latestBlock);

    // get the block by ID
    var blockByIdResult = await _flowClient.GetBlockByIdAsync(latestBlock.Id);
    PrintResult(blockByIdResult);

    // get block by height
    var blockByHeightResult = await _flowClient.GetBlockByHeightAsync(latestBlock.Height);
    PrintResult(blockByHeightResult);
}

private static void PrintResult(FlowBlock flowBlock)
{
    Console.WriteLine($"ID: {flowBlock.Id.FromByteStringToHex()}");
    Console.WriteLine($"height: {flowBlock.Height}");
    Console.WriteLine($"timestamp: {flowBlock.Timestamp}\n");            
}
```

Example output:
```bash
ID: 7bc42fe85d32ca513769a74f97f7e1a7bad6c9407f0d934c2aa645ef9cf613c7
height: 0
timestamp: 19/12/2018 22:32:30 +00:00

ID: 7bc42fe85d32ca513769a74f97f7e1a7bad6c9407f0d934c2aa645ef9cf613c7
height: 0
timestamp: 19/12/2018 22:32:30 +00:00

ID: 7bc42fe85d32ca513769a74f97f7e1a7bad6c9407f0d934c2aa645ef9cf613c7
height: 0
timestamp: 19/12/2018 22:32:30 +00:00
```

### Get Account
[<img src="https://raw.githubusercontent.com/onflow/sdks/main/templates/documentation/ref.svg" width="130">](https://tyronbrand.github.io/flow.net/api/Flow.Net.Sdk.Client.FlowClientAsync.html#Flow_Net_Sdk_Client_FlowClientAsync_GetAccountAtLatestBlockAsync_Google_Protobuf_ByteString_Grpc_Core_CallOptions_)

Retrieve any account from Flow network's latest block or from a specified block height.

📖 **Account address** is a unique account identifier. Be mindful about the `0x` prefix, you should use the prefix as a default representation but be careful and safely handle user inputs without the prefix.

An account includes the following data:
- Address: the account address.
- Balance: balance of the account.
- Contracts: list of contracts deployed to the account.
- Keys: list of keys associated with the account.

#### Examples
Example depicts ways to get an account at the latest block and at a specific block height:

**[<img src="https://raw.githubusercontent.com/onflow/sdks/main/templates/documentation/try.svg" width="130">](https://github.com/tyronbrand/flow.net/tree/main/examples/Flow.Net.Examples/AccountExamples/GetAccountExample.cs)**
```csharp
private static async Task Demo()
{
    // get account from the latest block
    var address = new FlowAddress("f8d6e0586b0a20c7");
    var account = await _flowClient.GetAccountAtLatestBlockAsync(address);
    PrintResult(account);

    // get account from the block by height 0
    account = await _flowClient.GetAccountAtBlockHeightAsync(address, 0);
    PrintResult(account);
}

private static void PrintResult(FlowAccount flowAccount)
{
    Console.WriteLine($"Address: {flowAccount.Address.HexValue}");
    Console.WriteLine($"Balance: {flowAccount.Balance}");
    Console.WriteLine($"Contracts: {flowAccount.Contracts.Count}");
    Console.WriteLine($"Keys: {flowAccount.Keys.Count}\n");
}
```

Example output:
```bash
Address: f8d6e0586b0a20c7
Balance: 999999999999700000
Contracts: 2
Keys: 1

Address: f8d6e0586b0a20c7
Balance: 999999999999700000
Contracts: 2
Keys: 1
```

### Get Transactions
[<img src="https://raw.githubusercontent.com/onflow/sdks/main/templates/documentation/ref.svg" width="130">](https://tyronbrand.github.io/flow.net/api/Flow.Net.Sdk.Client.FlowClientAsync.html#Flow_Net_Sdk_Client_FlowClientAsync_GetTransactionAsync_Google_Protobuf_ByteString_Grpc_Core_CallOptions_)

Retrieve transactions from the network by providing a transaction ID. After a transaction has been submitted, you can also get the transaction result to check the status.

📖 **Transaction ID** is a hash of the encoded transaction payload and can be calculated before submitting the transaction to the network.

⚠️ The transaction ID provided must be from the current spork.

📖 **Transaction status** represents the state of transaction in the blockchain. Status can change until is finalized.

| Status      | Final | Description |
| ----------- | ----------- | ----------- |
|   UNKNOWN    |    ❌   |   The transaction has not yet been seen by the network  |
|   PENDING    |    ❌   |   The transaction has not yet been included in a block   |
|   FINALIZED    |   ❌     |  The transaction has been included in a block   |
|   EXECUTED    |   ❌    |   The transaction has been executed but the result has not yet been sealed  |
|   SEALED    |    ✅    |   The transaction has been executed and the result is sealed in a block  |
|   EXPIRED    |   ✅     |  The transaction reference block is outdated before being executed    |


**[<img src="https://raw.githubusercontent.com/onflow/sdks/main/templates/documentation/try.svg" width="130">](https://github.com/tyronbrand/flow.net/tree/main/examples/Flow.Net.Examples/TransactionExamples/GetTransactionExample.cs)**
```csharp
private static async Task Demo(ByteString transactionId)
{
    var tx = await _flowClient.GetTransactionAsync(transactionId);
    PrintTransaction(tx);

    var txr = await _flowClient.GetTransactionResultAsync(transactionId);
    PrintTransactionResult(txr);
}

private static void PrintTransaction(FlowTransactionResponse tx)
{
    Console.WriteLine($"ReferenceBlockId: {tx.ReferenceBlockId.FromByteStringToHex()}");
    Console.WriteLine($"Payer: {tx.Payer.FromByteStringToHex()}");
    Console.WriteLine("Authorizers: [{0}]", string.Join(", ", tx.Authorizers.Select(s => s.FromByteStringToHex()).ToArray()));
    Console.WriteLine($"Proposer: {tx.ProposalKey.Address.FromByteStringToHex()}");
}

private static void PrintTransactionResult(FlowTransactionResult txr)
{
    Console.WriteLine($"Status: {txr.Status}");
    Console.WriteLine($"Error: {txr.ErrorMessage}\n");            
}
```

Example output:
```bash
ReferenceBlockId: a31c341a905dcfd16d8c0be0ebec389222572ff6dd2957bd944f55ee80c4dca9
Payer: f8d6e0586b0a20c7
Authorizers: []
Proposer: f8d6e0586b0a20c7
Status: Sealed
Error:
```

### Get Events
[<img src="https://raw.githubusercontent.com/onflow/sdks/main/templates/documentation/ref.svg" width="130">](https://tyronbrand.github.io/flow.net/api/Flow.Net.Sdk.Client.FlowClientAsync.html#Flow_Net_Sdk_Client_FlowClientAsync_GetEventsForBlockIdsAsync_System_String_System_Collections_Generic_IEnumerable_Google_Protobuf_ByteString__Grpc_Core_CallOptions_)

Retrieve events by a given type in a specified block height range or through a list of block IDs.

📖 **Event type** is a string that follow a standard format:
```
A.{contract address}.{contract name}.{event name}
```

Please read more about [events in the documentation](https://docs.onflow.org/core-contracts/flow-token/). The exception to this standard are 
core events, and you should read more about them in [this document](https://docs.onflow.org/cadence/language/core-events/).

📖 **Block height range** expresses the height of the start and end block in the chain.

#### Examples
Example depicts ways to get events within block range or by block IDs:

**[<img src="https://raw.githubusercontent.com/onflow/sdks/main/templates/documentation/try.svg" width="130">](https://github.com/tyronbrand/flow.net/tree/main/examples/Flow.Net.Examples/EventExamples/GetEventsExample.cs)**
```csharp
private static async Task Demo(FlowAccount flowAccount, ByteString flowTransactionId)
{
    // Query for account creation events by type
    var eventsForHeightRange = await _flowClient.GetEventsForHeightRangeAsync("flow.AccountCreated", 0, 100);
    PrintEvents(eventsForHeightRange);

    // Query for our custom event by type
    var customtype = $"A.{flowAccount.Address.HexValue}.EventDemo.Add";
    var customEventsForHeightRange = await _flowClient.GetEventsForHeightRangeAsync(customtype, 0, 100);
    PrintEvents(customEventsForHeightRange);

    // Get events directly from transaction result
    var txResult = await _flowClient.GetTransactionResultAsync(flowTransactionId);
    PrintEvent(txResult.Events);
}

private static void PrintEvents(IEnumerable<FlowBlockEvent> flowBlockEvents)
{
    foreach(var blockEvent in flowBlockEvents)
        PrintEvent(blockEvent.Events);
}

private static void PrintEvent(IEnumerable<FlowEvent> flowEvents)
{
    foreach(var @event in flowEvents)
    {
        Console.WriteLine($"Type: {@event.Type}");
        Console.WriteLine($"Values: {@event.Payload.Encode()}");
        Console.WriteLine($"Transaction ID: {@event.TransactionId.FromByteStringToHex()} \n");
    }
}
```

Example output:
```bash
Type: flow.AccountCreated
Values: {"type":"Event","value":{"id":"flow.AccountCreated","fields":[{"name":"address","value":{"type":"Address","value":"0x01cf0e2f2f715450"}}]}}
Transaction ID: 6edf928c88717fdaefe0849e014d7d4f7931471cdb6ae9329f992d4751844099

Type: A.01cf0e2f2f715450.EventDemo.Add
Values: {"type":"Event","value":{"id":"A.01cf0e2f2f715450.EventDemo.Add","fields":[{"name":"x","value":{"type":"Int","value":"2"}},{"name":"y","value":{"type":"Int","value":"3"}},{"name":"sum","value":{"type":"Int","value":"5"}}]}}
Transaction ID: 72ae51dbfcda12fdda9b97cf3e8df54980111c4b4bb7f0f86f9113420f21bece

Type: A.01cf0e2f2f715450.EventDemo.Add
Values: {"type":"Event","value":{"id":"A.01cf0e2f2f715450.EventDemo.Add","fields":[{"name":"x","value":{"type":"Int","value":"2"}},{"name":"y","value":{"type":"Int","value":"3"}},{"name":"sum","value":{"type":"Int","value":"5"}}]}}
Transaction ID: 72ae51dbfcda12fdda9b97cf3e8df54980111c4b4bb7f0f86f9113420f21bece
```

### Get Collections
[<img src="https://raw.githubusercontent.com/onflow/sdks/main/templates/documentation/ref.svg" width="130">](https://tyronbrand.github.io/flow.net/api/Flow.Net.Sdk.Client.FlowClientAsync.html#Flow_Net_Sdk_Client_FlowClientAsync_GetCollectionByIdAsync_Google_Protobuf_ByteString_Grpc_Core_CallOptions_)

Retrieve a batch of transactions that have been included in the same block, known as ***collections***. 
Collections are used to improve consensus throughput by increasing the number of transactions per block and they act as a link between a block and a transaction.

📖 **Collection ID** is SHA3-256 hash of the collection payload.

**[<img src="https://raw.githubusercontent.com/onflow/sdks/main/templates/documentation/try.svg" width="130">](https://github.com/tyronbrand/flow.net/tree/main/examples/Flow.Net.Examples/CollectionExamples/GetCollectionExample.cs)**
```csharp
private static async Task Demo(FlowCollectionGuarantee flowCollectionGuarantee)
{
    // get collection by ID
    var collection = await _flowClient.GetCollectionByIdAsync(flowCollectionGuarantee.CollectionId);
    PrintCollection(collection);
}

private static void PrintCollection(FlowCollectionResponse flowCollection)
{
    Console.WriteLine($"ID: {flowCollection.Id.FromByteStringToHex()}");
    Console.WriteLine("Transactions: [{0}]", string.Join(", ", flowCollection.TransactionIds.Select(s => s.FromByteStringToHex()).ToArray()));
}
```

Example output:
```bash
ID: 31a5c134b24fb556069575fa3acdfbdf6a0b4faf072df85c32ad476cba308468
Transactions: [6edf928c88717fdaefe0849e014d7d4f7931471cdb6ae9329f992d4751844099]
```

### Execute Scripts
[<img src="https://raw.githubusercontent.com/onflow/sdks/main/templates/documentation/ref.svg" width="130">](https://tyronbrand.github.io/flow.net/api/Flow.Net.Sdk.Client.FlowClientAsync.html#Flow_Net_Sdk_Client_FlowClientAsync_ExecuteScriptAtLatestBlockAsync_Google_Protobuf_ByteString_System_Collections_Generic_IEnumerable_Flow_Net_Sdk_Cadence_ICadence__Grpc_Core_CallOptions_)

Scripts allow you to write arbitrary non-mutating Cadence code on the Flow blockchain and return data. You can learn more about [Cadence and scripts here](https://docs.onflow.org/cadence/language/), but we are now only interested in executing the script code and getting back the data.

We can execute a script using the latest state of the Flow blockchain or we can choose to execute the script at a specific time in history defined by a block height or block ID.

📖 **Block ID** is SHA3-256 hash of the entire block payload, but you can get that value from the block response properties.

📖 **Block height** expresses the height of the block in the chain.

**[<img src="https://raw.githubusercontent.com/onflow/sdks/main/templates/documentation/try.svg" width="130">](https://github.com/tyronbrand/flow.net/tree/main/examples/Flow.Net.Examples/ScriptExamples/ScriptExample.cs)**
```csharp
private static async Task Demo()
{
    // simple script
    var script = @"
pub fun main(a: Int): Int {
    return a + 10
}";

    var arguments = new List<ICadence>
    {
        new CadenceNumber(CadenceNumberType.Int, "5")
    };

    var response = await FlowClient.ExecuteScriptAtLatestBlockAsync(script.FromStringToByteString(), arguments);
    Console.WriteLine($"Value: {response.As<CadenceNumber>().Value}");

    // complex script
    var complexScript = @"
pub struct User {
    pub var balance: UFix64
    pub var address: Address
    pub var name: String

    init(name: String, address: Address, balance: UFix64) {
        self.name = name
        self.address = address
        self.balance = balance
    }
}

pub fun main(name: String): User {
    return User(
        name: name,
        address: 0x1,
        balance: 10.0
    )
}";

    var complexArguments = new List<ICadence>
    {
        new CadenceString("Dete")
    };
    var complexResponse = await FlowClient.ExecuteScriptAtLatestBlockAsync(complexScript.FromStringToByteString(), complexArguments);
    PrintComplexScript(complexResponse);
}

public class User
{
    public decimal Balance { get; set; }
    public string Address { get; set; }
    public string Name { get; set; }
}

private static void PrintComplexScript(ICadence cadenceResponse)
{
    var user = new User
    {
        Name = cadenceResponse.As<CadenceComposite>().CompositeFieldAs<CadenceString>("name").Value,
        Address = cadenceResponse.As<CadenceComposite>().CompositeFieldAs<CadenceAddress>("address").Value.RemoveHexPrefix(),
        Balance = decimal.Parse(cadenceResponse.As<CadenceComposite>().CompositeFieldAs<CadenceNumber>("balance").Value)
    };

    Console.WriteLine($"Name: {user.Name}");
    Console.WriteLine($"Address: {user.Address}");
    Console.WriteLine($"Balance: {user.Balance}");
}
```

Example output:
```bash
Value: 15
Name: Dete
Address: 0000000000000001
Balance: 10.00000000
```

## Mutate Flow Network
Flow, like most blockchains, allows anybody to submit a transaction that mutates the shared global chain state. A transaction is an object that holds a payload, which describes the state mutation, and one or more authorizations that permit the transaction to mutate the state owned by specific accounts.

Transaction data is composed and signed with help of the SDK. The signed payload of transaction then gets submitted to the access node API. If a transaction is invalid or the correct number of authorizing signatures are not provided, it gets rejected. 

Executing a transaction requires couple of steps:
- [Building a transaction](#build-transactions).
- [Signing a transaction](#sign-transactions).
- [Sending a transaction](#send-transactions).

## Transactions
A transaction is nothing more than a signed set of data that includes script code which are instructions on how to mutate the network state and properties that define and limit it's execution. All these properties are explained bellow. 

📖 **Script** field is the portion of the transaction that describes the state mutation logic. On Flow, transaction logic is written in [Cadence](https://docs.onflow.org/cadence/). Here is an example transaction script:
```
transaction(greeting: String) {
  execute {
    log(greeting.concat(", World!"))
  }
}
```

📖 **Arguments**. A transaction can accept zero or more arguments that are passed into the Cadence script. The arguments on the transaction must match the number and order declared in the Cadence script. Sample script from above accepts a single `String` argument.

📖 **[Proposal key](https://docs.onflow.org/concepts/transaction-signing/#proposal-key)** must be provided to act as a sequence number and prevent reply and other potential attacks.

Each account key maintains a separate transaction sequence counter; the key that lends its sequence number to a transaction is called the proposal key.

A proposal key contains three fields:
- Account address
- Key index
- Sequence number

A transaction is only valid if its declared sequence number matches the current on-chain sequence number for that key. The sequence number increments by one after the transaction is executed.

📖 **[Payer](https://docs.onflow.org/concepts/transaction-signing/#signer-roles)** is the account that pays the fees for the transaction. A transaction must specify exactly one payer. The payer is only responsible for paying the network and gas fees; the transaction is not authorized to access resources or code stored in the payer account.

📖 **[Authorizers](https://docs.onflow.org/concepts/transaction-signing/#signer-roles)** are accounts that authorize a transaction to read and mutate their resources. A transaction can specify zero or more authorizers, depending on how many accounts the transaction needs to access.

The number of authorizers on the transaction must match the number of AuthAccount parameters declared in the prepare statement of the Cadence script.

Example transaction with multiple authorizers:
```
transaction {
  prepare(authorizer1: AuthAccount, authorizer2: AuthAccount) { }
}
```

📖 **Gas limit** is the limit on the amount of computation a transaction requires, and it will abort if it exceeds its gas limit.
Cadence uses metering to measure the number of operations per transaction. You can read more about it in the [Cadence documentation](/cadence).

The gas limit depends on the complexity of the transaction script. Until dedicated gas estimation tooling exists, it's best to use the emulator to test complex transactions and determine a safe limit.

📖 **Reference block** specifies an expiration window (measured in blocks) during which a transaction is considered valid by the network.
A transaction will be rejected if it is submitted past its expiry block. Flow calculates transaction expiry using the _reference block_ field on a transaction.
A transaction expires after `600` blocks are committed on top of the reference block, which takes about 10 minutes at average Mainnet block rates.

### Build Transactions
[<img src="https://raw.githubusercontent.com/onflow/sdks/main/templates/documentation/ref.svg" width="130">](https://tyronbrand.github.io/flow.net/api/Flow.Net.Sdk.Models.FlowTransaction.html)

Building a transaction involves setting the required properties explained above and producing a transaction object. 

Here we define a simple transaction script that will be used to execute on the network and serve as a good learning example.

```
transaction(greeting: String) {

  let guest: Address

  prepare(authorizer: AuthAccount) {
    self.guest = authorizer.address
  }

  execute {
    log(greeting.concat(",").concat(guest.toString()))
  }
}
```

**[<img src="https://raw.githubusercontent.com/onflow/sdks/main/templates/documentation/try.svg" width="130">](https://github.com/tyronbrand/flow.net/tree/main/examples/Flow.Net.Examples/TransactionExamples/CreateTransactionExample.cs)**
```csharp
private static async Task Demo()
{
    // reading script from folder
    var script = Utilities.ReadCadenceScript("greeting");

    var proposerAddress = new FlowAddress("9a0766d93b6608b7");
    uint proposerKeyIndex = 3;

    var payerAddress = new FlowAddress("631e88ae7f1d7c20");
    var authorizerAddress = new FlowAddress("7aad92e5a0715d21");

    // Establish a connection with an access node
    var accessAPIHost = "";
    var FlowClient = new FlowClientAsync(accessAPIHost);

    // Get the latest sealed block to use as a reference block
    var latestBlock = await FlowClient.GetLatestBlockHeaderAsync();

    // Get the latest account info for this address
    var proposerAccount = await FlowClient.GetAccountAtLatestBlockAsync(proposerAddress);

    // Get the latest sequence number for this key
    var proposerKey = proposerAccount.Keys.Where(w => w.Index == proposerKeyIndex).FirstOrDefault();
    var sequenceNumber = proposerKey.SequenceNumber;

    var tx = new FlowTransaction
    {
        Script = script,
        GasLimit = 100,
        ProposalKey = new FlowProposalKey
        {
            Address = proposerAddress.Value,
            KeyId = proposerKeyIndex,
            SequenceNumber = sequenceNumber
        },
        Payer = payerAddress.Value
    };

    // Add authorizer(s)
    tx.Authorizers.Add(authorizerAddress.Value);

    // Add argument(s)
    var arguments = new List<ICadence>
    {
        new CadenceString("Hello")
    };
    tx.Arguments = arguments.GenerateTransactionArguments();
}
```

After you have successfully [built a transaction](#build-transactions) the next step in the process is to sign it.

### Sign Transactions
[<img src="https://raw.githubusercontent.com/onflow/sdks/main/templates/documentation/ref.svg" width="130">](https://tyronbrand.github.io/flow.net/api/Flow.Net.Sdk.Models.FlowTransactionExtensions.html#Flow_Net_Sdk_Models_FlowTransactionExtensions_AddEnvelopeSignature_Flow_Net_Sdk_Models_FlowTransaction_Google_Protobuf_ByteString_System_UInt32_Flow_Net_Sdk_Crypto_ISigner_)

Flow introduces new concepts that allow for more flexibility when creating and signing transactions.
Before trying the examples below, we recommend that you read through the [transaction signature documentation](https://docs.onflow.org/concepts/accounts-and-keys/).

After you have successfully [built a transaction](#build-transactions) the next step in the process is to sign it. Flow transactions have envelope and payload signatures, and you should learn about each in the [signature documentation](https://docs.onflow.org/concepts/accounts-and-keys/#anatomy-of-a-transaction).

Quick example of building a transaction:
```csharp
var proposerAccount = new FlowAccount();
var proposerKey = proposerAccount.Keys.Where(w => w.Index == 1).FirstOrDefault();

var tx = new FlowTransaction
{
    Script = "transaction { execute { log(\"Hello, World!\") } }",
    GasLimit = 100,
    ProposalKey = new FlowProposalKey
    {
        Address = proposerAccount.Address.Value,
        KeyId = proposerKey.Index,
        SequenceNumber = proposerKey.SequenceNumber
    },
    Payer = proposerAccount.Address.Value
};
```

Signatures can be generated more securely using keys stored in a hardware device such as an [HSM](https://en.wikipedia.org/wiki/Hardware_security_module). The `ISigner` interface is intended to be flexible enough to support a variety of signer implementations and is not limited to in-memory implementations.

Simple signature example:
```csharp
// construct a signer from your private key and configured signature/hash algorithms
var signer = Sdk.Crypto.Ecdsa.Utilities.CreateSigner("privateKey", SignatureAlgo.ECDSA_P256, HashAlgo.SHA3_256);

tx.AddEnvelopeSignature(proposerAccount.Address, proposerKey.Index, signer);
```

Flow supports great flexibility when it comes to transaction signing, we can define multiple authorizers (multi-sig transactions) and have different payer account than proposer. We will explore advanced signing scenarios bellow.

### [Single party, single signature](https://docs.onflow.org/concepts/transaction-signing/#single-party-single-signature)

- Proposer, payer and authorizer are the same account (`0x01`).
- Only the envelope must be signed.
- Proposal key must have full signing weight.

| Account | Key ID | Weight |
| ------- | ------ | ------ |
| `0x01`  | 1      | 1.0    |

**[<img src="https://raw.githubusercontent.com/onflow/sdks/main/templates/documentation/try.svg" width="130">](https://github.com/tyronbrand/flow.net/tree/main/examples/Flow.Net.Examples/TransactionExamples/SinglePartySingleSignatureExample.cs)**
```csharp
// generate key
var flowAccountKey = FlowAccountKey.GenerateRandomEcdsaKey(SignatureAlgo.ECDSA_P256, HashAlgo.SHA3_256);
// create account with key
var account1 = await CreateAccountAsync(new List<FlowAccountKey> { flowAccountKey });
// select key
var account1Key = account1.Keys.FirstOrDefault();

// get the latest sealed block to use as a reference block
var lastestBlock = await FlowClient.GetLatestBlockAsync();

var tx = new FlowTransaction
{
    Script = "transaction {prepare(signer: AuthAccount) { log(signer.address) }}",
    GasLimit = 100,
    Payer = account1.Address.Value,
    ProposalKey = new FlowProposalKey
    {
        Address = account1.Address.Value,
        KeyId = account1Key.Index,
        SequenceNumber = account1Key.SequenceNumber
    },
    ReferenceBlockId = lastestBlock.Id
};

// authorizers
tx.Authorizers.Add(account1.Address.Value);

// account 1 signs the envelope with key 1
tx = FlowTransaction.AddEnvelopeSignature(tx, account1.Address, account1Key.Index, account1Key.Signer);

// send transaction
var txResponse = await FlowClient.SendTransactionAsync(tx);
```

### [Single party, multiple signatures](https://docs.onflow.org/concepts/transaction-signing/#single-party-multiple-signatures)

- Proposer, payer and authorizer are the same account (`0x01`).
- Only the envelope must be signed.
- Each key has weight 0.5, so two signatures are required.

| Account | Key ID | Weight |
| ------- | ------ | ------ |
| `0x01`  | 1      | 0.5    |
| `0x01`  | 2      | 0.5    |

**[<img src="https://raw.githubusercontent.com/onflow/sdks/main/templates/documentation/try.svg" width="130">](https://github.com/tyronbrand/flow.net/tree/main/examples/Flow.Net.Examples/TransactionExamples/SinglePartyMultiSignatureExample.cs)**
```csharp
// generate key 1 for account1
var flowAccountKey1 = FlowAccountKey.GenerateRandomEcdsaKey(SignatureAlgo.ECDSA_P256, HashAlgo.SHA3_256, 500);
// generate key 2 for account1
var flowAccountKey2 = FlowAccountKey.GenerateRandomEcdsaKey(SignatureAlgo.ECDSA_P256, HashAlgo.SHA3_256, 500);

// create account with keys
var account1 = await CreateAccountAsync(new List<FlowAccountKey> { flowAccountKey1, flowAccountKey2 });

// select keys
var account1Key1 = account1.Keys[0];
var account1Key2 = account1.Keys[1];

// get the latest sealed block to use as a reference block
var lastestBlock = await FlowClient.GetLatestBlockAsync();

var tx = new FlowTransaction
{
    Script = "transaction {prepare(signer: AuthAccount) { log(signer.address) }}",
    GasLimit = 9999,
    Payer = account1.Address.Value,
    ProposalKey = new FlowProposalKey
    {
        Address = account1.Address.Value,
        KeyId = account1Key1.Index,
        SequenceNumber = account1Key1.SequenceNumber
    },
    ReferenceBlockId = lastestBlock.Id
};

// authorizers
tx.Authorizers.Add(account1.Address.Value);

// account 1 signs the envelope with key 1
tx.AddEnvelopeSignature(account1.Address, account1Key1.Index, account1Key1.Signer);

// account 1 signs the envelope with key 2
tx.AddEnvelopeSignature(account1.Address, account1Key2.Index, account1Key2.Signer);

// send transaction
var txResponse = await FlowClient.SendTransactionAsync(tx);
```

### [Multiple parties](https://docs.onflow.org/concepts/transaction-signing/#multiple-parties)

- Proposer and authorizer are the same account (`0x01`).
- Payer is a separate account (`0x02`).
- Account `0x01` signs the payload.
- Account `0x02` signs the envelope.
    - Account `0x02` must sign last since it is the payer.

| Account | Key ID | Weight |
| ------- | ------ | ------ |
| `0x01`  | 1      | 1.0    |
| `0x02`  | 3      | 1.0    |

**[<img src="https://raw.githubusercontent.com/onflow/sdks/main/templates/documentation/try.svg" width="130">](https://github.com/tyronbrand/flow.net/tree/main/examples/Flow.Net.Examples/TransactionExamples/MultiPartySingleSignatureExample.cs)**
```csharp
// generate key for account1
var flowAccountKey1 = FlowAccountKey.GenerateRandomEcdsaKey(SignatureAlgo.ECDSA_P256, HashAlgo.SHA3_256);
// create account1
var account1 = await CreateAccountAsync(new List<FlowAccountKey> { flowAccountKey1 });
// select account1 key
var account1Key = account1.Keys.FirstOrDefault();

// generate key for account2
var flowAccountKey2 = FlowAccountKey.GenerateRandomEcdsaKey(SignatureAlgo.ECDSA_P256, HashAlgo.SHA3_256);
// create account2
var account2 = await CreateAccountAsync(new List<FlowAccountKey> { flowAccountKey2 });
// select account2 key
var account2Key = account2.Keys.FirstOrDefault();

// get the latest sealed block to use as a reference block
var lastestBlock = await FlowClient.GetLatestBlockAsync();

var tx = new FlowTransaction
{
    Script = "transaction {prepare(signer: AuthAccount) { log(signer.address) }}",
    GasLimit = 9999,
    Payer = account2.Address.Value,
    ProposalKey = new FlowProposalKey
    {
        Address = account1.Address.Value,
        KeyId = account1Key.Index,
        SequenceNumber = account1Key.SequenceNumber
    },
    ReferenceBlockId = lastestBlock.Id
};

// authorizers
tx.Authorizers.Add(account1.Address.Value);

// account 1 signs the payload with key 1
tx.AddPayloadSignature(account1.Address, account1Key.Index, account1Key.Signer);

// account 2 signs the envelope
tx.AddEnvelopeSignature(account2.Address, account2Key.Index, account2Key.Signer);

// send transaction
var txResponse = await FlowClient.SendTransactionAsync(tx);
```

### [Multiple parties, two authorizers](https://docs.onflow.org/concepts/transaction-signing/#multiple-parties)

- Proposer and authorizer are the same account (`0x01`).
- Payer is a separate account (`0x02`).
- Account `0x01` signs the payload.
- Account `0x02` signs the envelope.
    - Account `0x02` must sign last since it is the payer.
- Account `0x02` is also an authorizer to show how to include two AuthAccounts into an transaction

| Account | Key ID | Weight |
| ------- | ------ | ------ |
| `0x01`  | 1      | 1.0    |
| `0x02`  | 3      | 1.0    |

**[<img src="https://raw.githubusercontent.com/onflow/sdks/main/templates/documentation/try.svg" width="130">](https://github.com/tyronbrand/flow.net/tree/main/examples/Flow.Net.Examples/TransactionExamples/MultiPartyTwoAuthorizersExample.cs)**
```csharp
// generate key for account1
var flowAccountKey1 = FlowAccountKey.GenerateRandomEcdsaKey(SignatureAlgo.ECDSA_P256, HashAlgo.SHA3_256);
// create account1
var account1 = await CreateAccountAsync(new List<FlowAccountKey> { flowAccountKey1 });
// select account1 key
var account1Key = account1.Keys.FirstOrDefault();

// generate key for account2
var flowAccountKey2 = FlowAccountKey.GenerateRandomEcdsaKey(SignatureAlgo.ECDSA_P256, HashAlgo.SHA3_256);
// create account2
var account2 = await CreateAccountAsync(new List<FlowAccountKey> { flowAccountKey2 });
// select account2 key
var account2Key = account2.Keys.FirstOrDefault();

// get the latest sealed block to use as a reference block
var lastestBlock = await FlowClient.GetLatestBlockAsync();

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
    Payer = account2.Address.Value,
    ProposalKey = new FlowProposalKey
    {
        Address = account1.Address.Value,
        KeyId = account1Key.Index,
        SequenceNumber = account1Key.SequenceNumber
    },
    ReferenceBlockId = lastestBlock.Id
};

// authorizers
tx.Authorizers.Add(account1.Address.Value);
tx.Authorizers.Add(account2.Address.Value);

// account 1 signs the payload with key 1
tx.AddPayloadSignature(account1.Address, account1Key.Index, account1Key.Signer);

// account 2 signs the envelope
tx.AddEnvelopeSignature(account2.Address, account2Key.Index, account2Key.Signer);

// send transaction
var txResponse = await FlowClient.SendTransactionAsync(tx);
```

### [Multiple parties, multiple signatures](https://docs.onflow.org/concepts/transaction-signing/#multiple-parties)

- Proposer and authorizer are the same account (`0x01`).
- Payer is a separate account (`0x02`).
- Account `0x01` signs the payload.
- Account `0x02` signs the envelope.
    - Account `0x02` must sign last since it is the payer.
- Both accounts must sign twice (once with each of their keys).

| Account | Key ID | Weight |
| ------- | ------ | ------ |
| `0x01`  | 1      | 0.5    |
| `0x01`  | 2      | 0.5    |
| `0x02`  | 3      | 0.5    |
| `0x02`  | 4      | 0.5    |

**[<img src="https://raw.githubusercontent.com/onflow/sdks/main/templates/documentation/try.svg" width="130">](https://github.com/tyronbrand/flow.net/tree/main/examples/Flow.Net.Examples/TransactionExamples/MultiPartyMultiSignatureExample.cs)**
```csharp
// generate key 1 for account1
var flowAccountKey1 = FlowAccountKey.GenerateRandomEcdsaKey(SignatureAlgo.ECDSA_P256, HashAlgo.SHA3_256, 500);
// generate key 2 for account1
var flowAccountKey2 = FlowAccountKey.GenerateRandomEcdsaKey(SignatureAlgo.ECDSA_P256, HashAlgo.SHA3_256, 500);

// create account with keys
var account1 = await CreateAccountAsync(new List<FlowAccountKey> { flowAccountKey1, flowAccountKey2 });

// select keys
var account1Key1 = account1.Keys[0];
var account1Key2 = account1.Keys[1];

// get the latest sealed block to use as a reference block
var lastestBlock = await FlowClient.GetLatestBlockAsync();

var tx = new FlowTransaction
{
    Script = "transaction {prepare(signer: AuthAccount) { log(signer.address) }}",
    GasLimit = 9999,
    Payer = account1.Address.Value,
    ProposalKey = new FlowProposalKey
    {
        Address = account1.Address.Value,
        KeyId = account1Key1.Index,
        SequenceNumber = account1Key1.SequenceNumber
    },
    ReferenceBlockId = lastestBlock.Id
};

// authorizers
tx.Authorizers.Add(account1.Address.Value);

// account 1 signs the envelope with key 1
tx.AddEnvelopeSignature(account1.Address, account1Key1.Index, account1Key1.Signer);

// account 1 signs the envelope with key 2
tx.AddEnvelopeSignature(account1.Address, account1Key2.Index, account1Key2.Signer);

// send transaction
var txResponse = await FlowClient.SendTransactionAsync(tx);
```

### Send Transactions
[<img src="https://raw.githubusercontent.com/onflow/sdks/main/templates/documentation/ref.svg" width="130">](https://tyronbrand.github.io/flow.net/api/Flow.Net.Sdk.Client.FlowClientAsync.html#Flow_Net_Sdk_Client_FlowClientAsync_SendTransactionAsync_Flow_Net_Sdk_Models_FlowTransaction_Grpc_Core_CallOptions_)

After a transaction has been [built](#build-transactions) and [signed](#sign-transactions), it can be sent to the Flow blockchain where it will be executed. If sending was successful you can then [retrieve the transaction result](#get-transactions).

**[<img src="https://raw.githubusercontent.com/onflow/sdks/main/templates/documentation/try.svg" width="130">](https://github.com/tyronbrand/flow.net/tree/main/examples/Flow.Net.Examples/TransactionExamples/MultiPartyMultiSignatureExample.cs)**
```csharp
// send transaction
var txResponse = await _flowClient.SendTransactionAsync(tx);
```

### Create Accounts
[<img src="https://raw.githubusercontent.com/onflow/sdks/main/templates/documentation/ref.svg" width="130">](https://tyronbrand.github.io/flow.net/api/Flow.Net.Sdk.Templates.Account.html#Flow_Net_Sdk_Templates_Account_CreateAccount_System_Collections_Generic_IEnumerable_Flow_Net_Sdk_Models_FlowAccountKey__Google_Protobuf_ByteString_System_Collections_Generic_IEnumerable_Flow_Net_Sdk_Models_FlowContract__)

On Flow, account creation happens inside a transaction. Because the network allows for a many-to-many relationship between public keys and accounts, it's not possible to derive a new account address from a public key offline. 

The Flow VM uses a deterministic address generation algorithm to assigen account addresses on chain. You can find more details about address generation in the [accounts & keys documentation](https://docs.onflow.org/concepts/accounts-and-keys/).

#### Public Key
Flow uses ECDSA key pairs to control access to user accounts. Each key pair can be used in combination with the SHA2-256 or SHA3-256 hashing algorithms.

⚠️ You'll need to authorize at least one public key to control your new account.

Flow represents ECDSA public keys in raw form without additional metadata. Each key is a single byte slice containing a concatenation of its X and Y components in big-endian byte form.

A Flow account can contain zero (not possible to control) or more public keys, referred to as account keys. Read more about [accounts in the documentation](https://docs.onflow.org/concepts/accounts-and-keys/#accounts).

An account key contains the following data:
- Raw public key (described above)
- Signature algorithm
- Hash algorithm
- Weight (integer between 0-1000)

Account creation happens inside a transaction, which means that somebody must pay to submit that transaction to the network. We'll call this person the account creator. Make sure you have read [sending a transaction section](#send-transactions) first. 

**[<img src="https://raw.githubusercontent.com/onflow/sdks/main/templates/documentation/try.svg" width="130">](https://github.com/tyronbrand/flow.net/tree/main/examples/Flow.Net.Examples/ExampleBase.cs)**
```csharp
// keys
var flowAccountKey = FlowAccountKey.NewEcdsaAccountKey(SignatureAlgo.ECDSA_P256, HashAlgo.SHA3_256, 1000);
var newFlowAccountKeys = new List<FlowAccountKey> { flowAccountKey };

// creator key
var creatorAccount = new FlowAccount();
// creator key to use
var creatorAccountKey = creatorAccount.Keys.FirstOrDefault();

// use template to create a transaction
var tx = Account.CreateAccount(newFlowAccountKeys, creatorAccount.Address);

// set the transaction payer and proposal key
tx.Payer = creatorAccount.Address.Value;
tx.ProposalKey = new FlowProposalKey
{
    Address = creatorAccount.Address.Value,
    KeyId = creatorAccountKey.Index,
    SequenceNumber = creatorAccountKey.SequenceNumber
};

// get the latest sealed block to use as a reference block
var latestBlock = await _flowClient.GetLatestBlockAsync();
tx.ReferenceBlockId = latestBlock.Id;

// sign and submit the transaction
tx.AddEnvelopeSignature(creatorAccount.Address, creatorAccountKey.Index, creatorAccountKey.Signer);

await _flowClient.SendTransactionAsync(tx);
```

After the account creation transaction has been submitted you can retrieve the new account address by [getting the transaction result](#get-transactions). 

The new account address will be emitted in a system-level `flow.AccountCreated` event.

```csharp
 var result = await GetTransactionResultAsync(transactionResponse.Id);

if (result.Status == Sdk.Protos.entities.TransactionStatus.Sealed)
{
    var newAccountAddress = sealedResponse.Events.AccountCreatedAddress();

    // get new account details
    var newAccount = await FlowClient.GetAccountAtLatestBlockAsync(newAccountAddress);
    newAccount.Keys = FlowAccountKey.UpdateFlowAccountKeys(newFlowAccountKeys, newAccount.Keys);
    return newAccount;
}
```

### Generate Keys
[<img src="https://raw.githubusercontent.com/onflow/sdks/main/templates/documentation/ref.svg" width="130">](https://tyronbrand.github.io/flow.net/api/Flow.Net.Sdk.Crypto.Ecdsa.Utilities.html#Flow_Net_Sdk_Crypto_Ecdsa_Utilities_GenerateKeyPair_Flow_Net_Sdk_SignatureAlgo_)

Flow uses [ECDSA](https://en.wikipedia.org/wiki/Elliptic_Curve_Digital_Signature_Algorithm) signatures to control access to user accounts. Each key pair can be used in combination with the `SHA2-256` or `SHA3-256` hashing algorithms.

Here's how to generate an ECDSA private key for the P-256 (secp256r1) curve.

```csharp
var newKeys = Crypto.Ecdsa.Utilities.GenerateKeyPair(SignatureAlgo.ECDSA_P256);
var publicKey = Crypto.Ecdsa.Utilities.DecodePublicKeyToHex(newKeys);
var privateKey = Crypto.Ecdsa.Utilities.DecodePrivateKeyToHex(newKeys);
```

The example above uses an ECDSA key pair on the P-256 (secp256r1) elliptic curve. Flow also supports the secp256k1 curve used by Bitcoin and Ethereum. Read more about [supported algorithms here](https://docs.onflow.org/concepts/accounts-and-keys/#supported-signature--hash-algorithms).