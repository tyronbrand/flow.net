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
    public class UserSignatureValidateAllExample : ExampleBase
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
            var bobSigner = new Sdk.Core.Crypto.Ecdsa.Signer(flowAccountKeyBob.PrivateKey, flowAccountKeyBob.HashAlgorithm, flowAccountKeyBob.SignatureAlgorithm);

            var aliceSignature = UserMessage.Sign(message, aliceSigner);
            var bobSignature = UserMessage.Sign(message, bobSigner);

            var signatures = new CadenceArray(
                new List<ICadence>
                {new CadenceString(bobSignature.BytesToHex()),
                    new CadenceString(aliceSignature.BytesToHex())                    
                });

            // the signature indexes correspond to the key indexes on the address
            var signatureIndexes = new CadenceArray(
                new List<ICadence>
                {
                    new CadenceNumber(CadenceNumberType.Int, "1"),
                    new CadenceNumber(CadenceNumberType.Int, "0")
                });

            var script =  Utilities.ReadCadenceScript("user-signature-all-example");

            var response = await FlowClient.ExecuteScriptAtLatestBlockAsync(
                new FlowScript
                {
                    Script = script,
                    Arguments = new List<ICadence>
                    {
                        new CadenceAddress(flowAccount.Address.Address),
                        signatures,
                        signatureIndexes,
                        new CadenceString(Encoding.UTF8.GetString(message))
                    }
                });

            Console.WriteLine(response.As<CadenceBool>().Value
                ? "Signature verification succeeded"
                : "Signature verification failed");
        }
    }
}
