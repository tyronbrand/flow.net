## Executing a Script

You can use the `ExecuteScriptAtLatestBlockAsync` method to execute a read-only script against the latest sealed execution state.

This functionality can be used to read state from the blockchain.

Scripts must be in the following form:

- A single `main` function with a single return value

This is an example of a valid script:

```
pub fun main(): Int { return 1 + 1 }
```

```csharp
var _flowClient = FlowClientAsync.Create(networkUrl);

var script = "pub fun main(): Int { return 1 + 1 }".FromStringToByteString();
await _flowClient.ExecuteScriptAtLatestBlockAsync(script);
```

## Example

An example can be found [here](https://github.com/tyronbrand/flow.net/blob/main/examples/Flow.Net.Examples/ScriptExamples.cs).
