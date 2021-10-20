## Querying transaction

You can query transactions with the `GetTransactionAsync` method by Id.

```csharp
var _flowClient = FlowClientAsync.Create(networkUrl);

var result = await _flowClient.GetTransactionAsync(transactionId);
```