## Querying accounts

You can use the `GetAccountAtLatestBlockAsync` method to query the state of an account.

```csharp
var _flowClient = FlowClientAsync.Create(networkUrl);

// get account deatils
var accountDetails = await _flowClient.GetAccountAtLatestBlockAsync("0xAddress".FromHexToByteString());
```
## Example

An example can be found [here](https://github.com/tyronbrand/flow.net/blob/main/examples/Flow.Net.Examples/DeployUpdateDeleteContractExample.cs).
