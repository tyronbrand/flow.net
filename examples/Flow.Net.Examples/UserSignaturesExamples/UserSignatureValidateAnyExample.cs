using Flow.Net.Sdk.Core;
using Flow.Net.Sdk.Core.Cadence;
using Flow.Net.Sdk.Core.Client;
using Flow.Net.Sdk.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Flow.Net.Examples.UserSignaturesExamples
{
    public class UserSignatureValidateAnyExample : ExampleBase
    {
        public static async Task RunAsync(IFlowClient flowClient)
        {
            Console.WriteLine("\nRunning UserSignatureExample\n");
            FlowClient = flowClient;
            await Demo();
            Console.WriteLine("\nUserSignatureExample Complete\n");
        }

        private static async Task Demo()
        {
            // create the keys
            var flowAccountKeyAlice = FlowAccountKey.GenerateRandomEcdsaKey(SignatureAlgo.ECDSA_P256, HashAlgo.SHA3_256, 500);
            var flowAccountKeyBob = FlowAccountKey.GenerateRandomEcdsaKey(SignatureAlgo.ECDSA_P256, HashAlgo.SHA3_256, 500);

            // create the message that will be signed
            var flowAccount = await CreateAccountAsync(new List<FlowAccountKey> { flowAccountKeyAlice, flowAccountKeyBob });

            var message = Encoding.UTF8.GetBytes("ananas");

            // sign the message
            var aliceSigner = new Sdk.Core.Crypto.Ecdsa.Signer(flowAccountKeyAlice.PrivateKey, flowAccountKeyAlice.HashAlgorithm, flowAccountKeyAlice.SignatureAlgorithm);
            var aliceSignature = UserMessage.Sign(message, aliceSigner);            

            var script =  Utilities.ReadCadenceScript("user-signature-any-example");

            var response = await FlowClient.ExecuteScriptAtLatestBlockAsync(
                new FlowScript
                {
                    Script = script,
                    Arguments = new List<ICadence>
                    {
                        new CadenceAddress(flowAccount.Address.Address),
                        new CadenceString(aliceSignature.BytesToHex()),
                        new CadenceString(Encoding.UTF8.GetString(message))
                    }
                });

            Console.WriteLine(response.As<CadenceBool>().Value
                ? "Signature verification succeeded"
                : "Signature verification failed");
        }
    }
}
