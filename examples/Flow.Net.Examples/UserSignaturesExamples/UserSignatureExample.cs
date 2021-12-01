using Flow.Net.Sdk;
using Flow.Net.Sdk.Cadence;
using Flow.Net.Sdk.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Flow.Net.Examples.UserSignaturesExamples
{
    public class UserSignatureExample : ExampleBase
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
            var flowAccountKeyAlice = FlowAccountKey.GenerateRandomEcdsaKey(SignatureAlgo.ECDSA_P256, HashAlgo.SHA3_256);
            var flowAccountKeyBob = FlowAccountKey.GenerateRandomEcdsaKey(SignatureAlgo.ECDSA_P256, HashAlgo.SHA3_256);

            // create the message that will be signed
            var aliceFlowAccount = await CreateAccountAsync(new List<FlowAccountKey> { flowAccountKeyAlice });
            var bobFlowAccount = await CreateAccountAsync(new List<FlowAccountKey> { flowAccountKeyBob });

            var toAddress = new CadenceAddress(aliceFlowAccount.Address.HexValue);
            var fromAddress = new CadenceAddress(bobFlowAccount.Address.HexValue);
            var amount = new CadenceNumber(CadenceNumberType.UInt64, "100");

            var message = Utilities.CombineByteArrays(new[]
            {
                aliceFlowAccount.Address.HexValue.FromHexToBytes(),
                bobFlowAccount.Address.HexValue.FromHexToBytes()
            });

            var amountBytes = BitConverter.GetBytes(ulong.Parse(amount.Value));
            amountBytes = BitConverter.IsLittleEndian ? amountBytes.Reverse().ToArray() : amountBytes;

            message = Utilities.CombineByteArrays(new[]
            {
                message,
                amountBytes
            });

            // sign the message with Alice and Bob
            var aliceSigner = new Sdk.Crypto.Ecdsa.Signer(flowAccountKeyAlice.PrivateKey, flowAccountKeyAlice.HashAlgorithm, flowAccountKeyAlice.SignatureAlgorithm);
            var bobSigner = new Sdk.Crypto.Ecdsa.Signer(flowAccountKeyBob.PrivateKey, flowAccountKeyBob.HashAlgorithm, flowAccountKeyBob.SignatureAlgorithm);

            var aliceSignature = UserMessage.Sign(message, aliceSigner);
            var bobSignature = UserMessage.Sign(message, bobSigner);

            var publicKeys = new CadenceArray(
                new List<ICadence>
                {
                    new CadenceString(flowAccountKeyAlice.PublicKey),
                    new CadenceString(flowAccountKeyBob.PublicKey)
                });

            // each signature has half weight
            var weightAlice = new CadenceNumber(CadenceNumberType.UFix64, "0.5");
            var weightBob = new CadenceNumber(CadenceNumberType.UFix64, "0.5");

            var weights = new CadenceArray(
                new List<ICadence>
                {
                    weightAlice,
                    weightBob
                });

            var signatures = new CadenceArray(
                new List<ICadence>
                {
                    new CadenceString(aliceSignature.FromByteArrayToHex()),
                    new CadenceString(bobSignature.FromByteArrayToHex())
                });

            var script =  Utilities.ReadCadenceScript("user-signature-example").FromStringToByteString();

            var response = await FlowClient.ExecuteScriptAtLatestBlockAsync(
                script,
                new List<ICadence>
                {
                    publicKeys,
                    weights,
                    signatures,
                    toAddress,
                    fromAddress,
                    amount,
                });

            Console.WriteLine(response.As<CadenceBool>().Value
                ? "Signature verification succeeded"
                : "Signature verification failed");
        }
    }
}
