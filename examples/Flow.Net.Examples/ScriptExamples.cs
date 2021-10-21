﻿using Flow.Net.Sdk.Extensions;
using Flow.Net.Sdk.Client;
using Flow.Net.Sdk.Cadence;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Flow.Net.Examples
{
    public class ScriptExamples
    {
        public static FlowClientAsync _flowClient;

        public static async Task RunAsync()
        {
            var networkUrl = "127.0.0.1:3569"; // emulator

            _flowClient = FlowClientAsync.Create(networkUrl);

            await _flowClient.PingAsync();

            // examples
            await ExecuteScriptAtLatestBlockExampleWithParameters();
            await ExecuteScriptAtLatestBlockExample();
            await ExecuteScriptAtBlockHeightExample();
            await ExecuteScriptAtBlockIdExample();
        }

        public static async Task ExecuteScriptAtLatestBlockExampleWithParameters()
        {
            var script = "pub fun main(num: Int32): Int32 { return 54534 + num }".FromStringToByteString();
            var arguments = new List<ICadence>
            {
                new CadenceNumber(CadenceNumberType.Int32, "834534")
            };

            var response = await _flowClient.ExecuteScriptAtLatestBlockAsync(script, arguments);

            var cadenceNumber = response.AsCadenceType<CadenceNumber>();
            cadenceNumber.Value = "123";

            Console.WriteLine(JsonConvert.SerializeObject(cadenceNumber));
        }

        public static async Task ExecuteScriptAtLatestBlockExample()
        {
            var script = "pub fun main(): Int { return 1 + 1 }".FromStringToByteString();

            var response = await _flowClient.ExecuteScriptAtLatestBlockAsync(script);

            Console.WriteLine(JsonConvert.SerializeObject(response));
        }

        public static async Task ExecuteScriptAtBlockHeightExample()
        {
            var script = "pub fun main(): Int { return 1 + 2 }".FromStringToByteString();

            var latestBlock = await _flowClient.GetLatestBlockAsync(); // getting a height for example purpose
            var response = await _flowClient.ExecuteScriptAtBlockHeightAsync(script, latestBlock.Height);

            Console.WriteLine(JsonConvert.SerializeObject(response));
        }

        public static async Task ExecuteScriptAtBlockIdExample()
        {
            var script = "pub fun main(): Int { return 1 + 3 }".FromStringToByteString();

            var latestBlock = await _flowClient.GetLatestBlockAsync(); // getting a height for example purpose
            var response = await _flowClient.ExecuteScriptAtBlockIdAsync(script, latestBlock.Id);

            Console.WriteLine(JsonConvert.SerializeObject(response));
        }
    }
}