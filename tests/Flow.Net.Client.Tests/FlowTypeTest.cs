using Flow.Net.Client.Models;
using Flow.Net.Client.JsonCadence;
using Flow.Net.Sdk;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Flow.Net.Client.Tests
{
    public class FlowTypeTest
    {
        [Fact]
        public void FromStringToJson()
        {
            var json = "{\"type\": \"String\",\"value\": \"test test test test test test test test \"}";
            var result = JsonConvert.DeserializeObject<IFlowType>(json, new JsonCadenceConverter());

            var testList = new List<IFlowType>();
            var item1 = new FlowString("teste");
            var item2 = new FlowBool(true);
            testList.Add(item1);
            testList.Add(item2);

            var result2 = JsonConvert.SerializeObject(testList);

            var json3 = "{\"type\": \"Array\",\"value\":[{\"type\":\"UInt32\",\"value\":\"123\"},{\"type\":\"String\",\"value\":\"test\"},{\"type\":\"Bool\",\"value\":true}]}";
            var result3 = JsonConvert.DeserializeObject<IFlowType>(json3, new JsonCadenceConverter());

            var json4 = "{\"type\": \"Dictionary\",\"value\": [{\"key\": {\"type\": \"UInt32\", \"value\": \"123\"},\"value\": {\"type\": \"String\", \"value\": \"test\"}}]}";
            var result4 = JsonConvert.DeserializeObject<IFlowType>(json4, new JsonCadenceConverter());
                        
            var args = new List<IFlowType> {
                new FlowString("Tessdjhfsdjhf"),
                new FlowNumber(FlowNumberType.Int32, "123"),
                new FlowArray(new List<IFlowType>()),
                new FlowDictionary(new List<FlowDictionaryItem>())
            };

            var stringArgs = new List<string>();
            foreach (var item in args)
            {
                stringArgs.Add(JsonConvert.SerializeObject(item));
            }

            var flowNumber = new FlowNumber(FlowNumberType.Int32, "123");
            var asjdhas = JsonConvert.SerializeObject(flowNumber);

        }
    }
}
