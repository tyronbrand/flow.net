## Sending a transaction

Submit a transaction using the `SendTransactionAsync` method.

```csharp
var _flowClient = FlowClientAsync.Create(networkUrl);

var transaction = new FlowTransaction();
var result = await _flowClient.SendTransactionAsync(transaction);
```

## Example

An example can be found [here](https://github.com/tyronbrand/flow.net/blob/main/examples/Flow.Net.Examples/TransactionExamples.cs).