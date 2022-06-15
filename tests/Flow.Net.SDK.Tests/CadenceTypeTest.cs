using ExpectedObjects;
using Flow.Net.Sdk.Cadence;
using Flow.Net.Sdk.Cadence.Types;
using Newtonsoft.Json;
using System.Collections.Generic;
using Xunit;

namespace Flow.Net.Sdk.Tests
{
    public class CadenceTypeTest
    {
        private readonly CadenceConverter _cadenceConverter;
        private readonly CadenceTypeConverter _cadenceTypeConverter;

        public CadenceTypeTest()
        {
            _cadenceConverter = new CadenceConverter();
            _cadenceTypeConverter = new CadenceTypeConverter();
        }

        [Fact]
        public void CadenceTypes()
        {
            var cadenceTypes = new List<string>
            {
                "Any","AnyStruct","AnyResource","Type",
                "Void","Never","Bool","String","Character",
                "Bytes","Address","Number","SignedNumber",
                "Integer","SignedInteger","FixedPoint",
                "SignedFixedPoint","Int","Int8","Int16",
                "Int32","Int64","Int128","Int256","UInt",
                "UInt8","UInt16","UInt32","UInt64","UInt128",
                "UInt256","Word8","Word16","Word32","Word64",
                "Fix64","UFix64","Path","CapabilityPath","StoragePath",
                "PublicPath","PrivatePath","AuthAccount","PublicAccount",
                "AuthAccount.Keys","PublicAccount.Keys","AuthAccount.Contracts",
                "PublicAccount.Contracts","DeployedContract","AccountKey","Block"
            };

            var testItems = new List<CadenceTestItem>();

            foreach(var type in cadenceTypes)
            {
                testItems.Add(new CadenceTestItem
                {
                    Name = $"Cadence{type}Type",
                    CadenceType = new CadenceType
                    {
                        Kind = type
                    },
                    ExpectedJson = "{\"kind\":\"" + type + "\"}"
                });
            };           

            TestEncodeAndDecode(testItems);
        }

        [Fact]
        public void CadenceOptionalType()
        {
            var testItems = new List<CadenceTestItem>
            {
                new ()
                {
                    Name = "CadenceOptionalType",
                    CadenceType =  new CadenceOptionalType
                    {
                        Type = new CadenceType
                        {
                            Kind = "String"
                        }
                    },
                    ExpectedJson = "{\"kind\":\"Optional\",\"type\":{\"kind\":\"String\"}}"
                }
            };

            TestEncodeAndDecode(testItems);
        }

        [Fact]
        public void CadenceVariableSizedArrayTypes()
        {
            var testItems = new List<CadenceTestItem>
            {
                new ()
                {
                    Name = "CadenceVariableSizedArrayType",
                    CadenceType =  new CadenceVariableSizedArrayType
                    {
                        Type = new CadenceType
                        {
                            Kind = "String"
                        }
                    },
                    ExpectedJson = "{\"kind\":\"VariableSizedArray\",\"type\":{\"kind\":\"String\"}}"
                }
            };

            TestEncodeAndDecode(testItems);

            TestEncodeAndDecode(testItems);
        }

        private void TestEncodeAndDecode(IEnumerable<CadenceTestItem> testItems)
        {
            foreach (var testItem in testItems)
            {
                var encodeResult = TestEncode(testItem.CadenceType, testItem.ExpectedJson);
                TestDecode(encodeResult, testItem.CadenceType);
            }
        }

        private static string TestEncode(ICadenceType cadenceType, string expectedJson)
        {
            var encoded = cadenceType.Encode(cadenceType);
            Assert.Equal(expectedJson, encoded);
            return encoded;
        }

        private void TestDecode(string actualJson, ICadenceType cadenceType)
        {
            JsonConverter[] jsonConverters = { _cadenceConverter, _cadenceTypeConverter };
            var decoded = JsonConvert.DeserializeObject<ICadenceType>(actualJson, jsonConverters);
            cadenceType.ToExpectedObject().ShouldEqual(decoded);
        }

        private class CadenceTestItem
        {
            public string Name { get; set; }
            public ICadenceType CadenceType { get; init; }
            public string ExpectedJson { get; init; }
        }
    }
}
