using Flow.Net.Sdk;
using Flow.Net.Sdk.Cadence;
using Flow.Net.Sdk.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Flow.Net.Examples.UserSignaturesExamples
{
    public class UserSignatureValidateAnyExample : ExampleBase
    {
        public static async Task RunAsync()
        {
            Console.WriteLine("\nRunning UserSignatureExample\n");
            await CreateFlowClientAsync();
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
            var aliceSigner = new Sdk.Crypto.Ecdsa.Signer(flowAccountKeyAlice.PrivateKey, flowAccountKeyAlice.HashAlgorithm, flowAccountKeyAlice.SignatureAlgorithm);
            var aliceSignature = UserMessage.Sign(message, aliceSigner);            

            var script =  Utilities.ReadCadenceScript("user-signature-any-example").FromStringToByteString();

            var response = await FlowClient.ExecuteScriptAtLatestBlockAsync(
                script,
                new List<ICadence>
                {
                    new CadenceAddress(flowAccount.Address.HexValue),
                    new CadenceString(aliceSignature.FromByteArrayToHex()),
                    new CadenceString(Encoding.UTF8.GetString(message))
                });

            Console.WriteLine(response.As<CadenceBool>().Value
                ? "Signature verification succeeded"
                : "Signature verification failed");
        }
    }
}
