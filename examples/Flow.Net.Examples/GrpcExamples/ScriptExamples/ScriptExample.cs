using Flow.Net.Examples.GrpcExamples;
using Flow.Net.Sdk;
using Flow.Net.Sdk.Cadence;
using Flow.Net.Sdk.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Flow.Net.Examples.GrpcExamples.ScriptExamples
{
    public class ScriptExample : GrpcExampleBase
    {
        public static async Task RunAsync()
        {
            Console.WriteLine("\nRunning ScriptExample\n");
            await CreateFlowClientAsync();
            await Demo();
            Console.WriteLine("\nScriptExample Complete\n");
        }

        private static async Task Demo()
        {
            // simple script
            var script = @"
pub fun main(a: Int): Int {
    return a + 10
}";

            var arguments = new List<ICadence>
            {
                new CadenceNumber(CadenceNumberType.Int, "5")
            };

            var response = await FlowClient.ExecuteScriptAtLatestBlockAsync(
                new FlowScript
                {
                    Script = script,
                    Arguments = arguments
                });
            Console.WriteLine($"Value: {response.As<CadenceNumber>().Value}");

            // complex script
            var complexScript = @"
pub struct User {
    pub var balance: UFix64
    pub var address: Address
    pub var name: String

    init(name: String, address: Address, balance: UFix64) {
        self.name = name
        self.address = address
        self.balance = balance
    }
}

pub fun main(name: String): User {
    return User(
        name: name,
        address: 0x1,
        balance: 10.0
    )
}";

            var complexArguments = new List<ICadence>
            {
                new CadenceString("Dete")
            };
            var complexResponse = await FlowClient.ExecuteScriptAtLatestBlockAsync(
                new FlowScript
                {
                    Script = complexScript,
                    Arguments = complexArguments
                });
            PrintComplexScript(complexResponse);
        }

        public class User
        {
            public decimal Balance { get; set; }
            public string Address { get; set; }
            public string Name { get; set; }
        }

        private static void PrintComplexScript(ICadence cadenceResponse)
        {
            var user = new User
            {
                Name = cadenceResponse.As<CadenceComposite>().CompositeFieldAs<CadenceString>("name").Value,
                Address = cadenceResponse.As<CadenceComposite>().CompositeFieldAs<CadenceAddress>("address").Value.RemoveHexPrefix(),
                Balance = decimal.Parse(cadenceResponse.As<CadenceComposite>().CompositeFieldAs<CadenceNumber>("balance").Value)
            };

            Console.WriteLine($"Name: {user.Name}");
            Console.WriteLine($"Address: {user.Address}");
            Console.WriteLine($"Balance: {user.Balance}");
        }
    }
}
