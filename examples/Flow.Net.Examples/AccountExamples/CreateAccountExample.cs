using Flow.Net.Sdk;
using Flow.Net.Sdk.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Flow.Net.Examples
{
    public class CreateAccountExample : ExampleBase
    {
        public static async Task RunAsync()
        {
            // generate our new account key
            var flowAccountKey = FlowAccountKey.NewEcdsaAccountKey(SignatureAlgo.ECDSA_P256, HashAlgo.SHA3_256, 1000);

            // example found in base class
            await CreateAccountAsync(new List<FlowAccountKey> { flowAccountKey });
        }
    }
}
