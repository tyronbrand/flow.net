using Flow.Net.Sdk;
using Flow.Net.Sdk.Cadence;
using Flow.Net.Sdk.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Flow.Net.Examples.UserSignaturesExamples
{
    public class UserSignatureValidateAllExample : GrpcExampleBase
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
            var bobSigner = new Sdk.Crypto.Ecdsa.Signer(flowAccountKeyBob.PrivateKey, flowAccountKeyBob.HashAlgorithm, flowAccountKeyBob.SignatureAlgorithm);

            var aliceSignature = UserMessage.Sign(message, aliceSigner);
            var bobSignature = UserMessage.Sign(message, bobSigner);

            var signatures = new CadenceArray(
                new List<ICadence>
                {new CadenceString(bobSignature.FromByteArrayToHex()),
                    new CadenceString(aliceSignature.FromByteArrayToHex())                    
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
                        new CadenceAddress(flowAccount.Address.HexValue),
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
