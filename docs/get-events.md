## Querying Events

You can use the `GetEventsForHeightRangeAsync` query events.

```csharp
var _flowClient = FlowClientAsync.Create(networkUrl);

var response = await _flowClient.GetEventsForHeightRangeAsync(eventType : "flow.AccountCreated", startHeight: 10, endHeight: 15);
```

## Example

An example can be found [here](https://github.com/tyronbrand/flow.net/blob/main/examples/Flow.Net.Examples/GetEventsForHeightRangeExample.cs).