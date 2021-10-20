## Querying transaction result

After you have submitted a transaction, you can use the `GetTransactionResultAsync` method to query its status by Id.

```csharp
var _flowClient = FlowClientAsync.Create(networkUrl);

var result = await _flowClient.GetTransactionResultAsync(transactionResponse.Id);
```

The result includes a `Status` field that will be one of the following values:

- `UNKNOWN` - The transaction has not yet been seen by the network.
- `PENDING` - The transaction has not yet been included in a block.
- `FINALIZED` - The transaction has been included in a block.
- `EXECUTED` - The transaction has been executed but the result has not yet been sealed.
- `SEALED` - The transaction has been executed and the result is sealed in a block.

```csharp
if (result.Status == Protos.entities.TransactionStatus.Sealed)
{
    // Transaction is sealed!
}
```
