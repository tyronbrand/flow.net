using Flow.Net.Sdk;
using Flow.Net.Sdk.Cadence;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Flow.Net.Examples
{
    public class ScriptExample : ExampleBase
    {
        public static async Task RunAsync()
        {
            await CreateFlowClientAsync();
            await Demo();
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

            var response = await _flowClient.ExecuteScriptAtLatestBlockAsync(script.FromStringToByteString(), arguments);
            Console.WriteLine($"Value: {response.AsCadenceType<CadenceNumber>().Value}");

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
            var complexResponse = await _flowClient.ExecuteScriptAtLatestBlockAsync(complexScript.FromStringToByteString(), complexArguments);
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
                Name = cadenceResponse.AsCadenceType<CadenceComposite>().CadenceCompositeValueAsCadenceType<CadenceString>("name").Value,
                Address = cadenceResponse.AsCadenceType<CadenceComposite>().CadenceCompositeValueAsCadenceType<CadenceAddress>("address").Value.Remove0x(),
                Balance = decimal.Parse(cadenceResponse.AsCadenceType<CadenceComposite>().CadenceCompositeValueAsCadenceType<CadenceNumber>("balance").Value)
            };

            Console.WriteLine($"Name: {user.Name}");
            Console.WriteLine($"Address: {user.Address}");
            Console.WriteLine($"Balance: {user.Balance}");
        }
    }
}
