using Flow.Net.Sdk;
using Flow.Net.Sdk.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Flow.Net.Examples
{
    public class CreateAccountExample : ExampleBase
    {
        public static async Task RunAsync()
        {
            ColorConsole.WriteWrappedHeader("Create account example", headerColor: ConsoleColor.Yellow, dashColor: ConsoleColor.Yellow);

            // generate our new account key
            var flowAccountKey = FlowAccountKey.NewEcdsaAccountKey(SignatureAlgo.ECDSA_P256, HashAlgo.SHA3_256, 1000);

            ColorConsole.WriteWrappedSubHeader("Creating new account");
            // example found in base class
            await CreateAccountAsync(new List<FlowAccountKey> { flowAccountKey });

            ColorConsole.WriteWrappedHeader("End create account example", headerColor: ConsoleColor.Yellow, dashColor: ConsoleColor.Gray);
        }
    }
}
