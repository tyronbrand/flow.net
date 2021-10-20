## Querying blocks

You can use the `GetLatestBlockAsync` method to fetch the latest sealed or unsealed block.

```csharp
var _flowClient = FlowClientAsync.Create(networkUrl);

// fetch the latest sealed block
var latestSealedBlock = await _flowClient.GetLatestBlockAsync();

// fetch the latest unsealed block
bool isSealed = false;
var latestUnsealedBlock = await _flowClient.GetLatestBlockAsync(isSealed);
```

## Example

An example can be found [here](https://github.com/tyronbrand/flow.net/blob/main/examples/Flow.Net.Examples/BlockExamples.cs).