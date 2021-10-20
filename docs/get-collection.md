## Querying collection

You can query transactions with the `GetCollectionByIdAsync` method by collection Id.

```csharp
var _flowClient = FlowClientAsync.Create(networkUrl);

var result = await _flowClient.GetCollectionByIdAsync(collectionId);
```