using Flow.Net.Sdk.Cadence;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Flow.Net.Sdk.Tests
{
    public class CadenceTest
    {
        [Fact]
        public void TestCadenceAddressDecode()
        {
            var cadence = new CadenceAddress("0x871293871293");
            var cadenceDecoded = cadence.Decode();

            Assert.Equal("0x871293871293", cadenceDecoded);
        }

        [Fact]
        public void TestCadenceBoolDecode()
        {
            var cadence = new CadenceBool(false);
            var cadenceDecoded = cadence.Decode();

            Assert.Equal(false, cadenceDecoded);
        }

        [Fact]
        public void TestCadenceCapabilityDecode()
        {
            var capabilityValue = new FlowCapabilityValue
            {
                Address = "add",
                BorrowType = "borrow",
                Path = "path"
            };
            var cadence = new CadenceCapability(capabilityValue);
            var cadenceDecoded = cadence.Decode();

            Assert.Equal(capabilityValue, cadenceDecoded);
        }

        [Fact]
        public void TestCadenceOptionalDecode()
        {
            var cadence = new CadenceOptional(new CadenceAddress("0x45866567458"));
            var cadenceDecoded = cadence.Decode();

            Assert.Equal("0x45866567458", cadenceDecoded);
        }

        [Fact]
        public void TestCadenceOptionalNullDecode()
        {
            var cadence = new CadenceOptional();
            var cadenceDecoded = cadence.Decode();

            Assert.Null(cadenceDecoded);
        }

        [Fact]
        public void TestCadencePathDecode()
        {
            var pathValue = new FlowPathValue
            {
                Domain = "dom",
                Identifier = "id"
            };
            var cadence = new CadencePath(pathValue);
            var cadenceDecoded = cadence.Decode();

            Assert.Equal(pathValue, cadenceDecoded);
        }

        [Fact]
        public void TestCadenceStringDecode()
        {
            var cadence = new CadenceString("Test String");
            var cadenceDecoded = cadence.Decode();

            Assert.Equal("Test String", cadenceDecoded);
        }

        [Fact]
        public void TestCadenceStringEmptyDecode()
        {
            var cadence = new CadenceString("");
            var cadenceDecoded = cadence.Decode();

            Assert.Equal("", cadenceDecoded);
        }

        [Fact]
        public void TestCadenceTypeDecode()
        {
            var typeValue = new FlowTypeValue
            {
                StaticType = "Int"
            };
            var cadence = new CadenceType(typeValue);
            var cadenceDecoded = cadence.Decode();

            Assert.Equal(typeValue, cadenceDecoded);
        }

        [Fact]
        public void TestCadenceVoidDecode()
        {
            var cadence = new CadenceVoid();
            var cadenceDecoded = cadence.Decode();

            Assert.Null(cadenceDecoded);
        }        

        [Fact]
        public void TestCadenceArrayDecode()
        {
            var cadenceArray = new CadenceArray(
                new List<ICadence>
                {
                    new CadenceNumber(CadenceNumberType.Int32, "1726387"),
                    new CadenceBool(true),
                    new CadenceNumber(CadenceNumberType.UInt16, "65530")
                });

            var cadenceDecoded = cadenceArray.Decode();
            Assert.True(cadenceDecoded.GetType().IsArray);

            // checking values and Types
            var cadenceDecodedAsArray = ((IEnumerable)cadenceDecoded).Cast<object>().ToArray();

            Assert.Equal(1726387, cadenceDecodedAsArray[0]);
            Assert.Equal(true, cadenceDecodedAsArray[1]);
            Assert.Equal(Convert.ToUInt16(65530), cadenceDecodedAsArray[2]);
        }

        [Fact]
        public void TestCadenceDictionaryDecode()
        {
            var cadenceArray = new CadenceDictionary(
                new List<FlowDictionaryItem>
                {
                    new FlowDictionaryItem 
                    {
                        Key = new CadenceNumber(CadenceNumberType.Int32, "1726387"),
                        Value = new CadenceBool(true)
                    },
                    new FlowDictionaryItem
                    {
                        Key = new CadenceNumber(CadenceNumberType.Int64, "1726387"),
                        Value = new CadenceAddress("0x192837123")
                    },
                    new FlowDictionaryItem
                    {
                        Key = new CadenceString("ImKey"),
                        Value = new CadenceVoid()
                    }
                });

            var cadenceDecoded = cadenceArray.Decode();

            // checking values and Types
            var cadenceDecodedAsArray = ((IEnumerable)cadenceDecoded).Cast<object>().ToArray();

            for (var i = 0; i < cadenceDecodedAsArray.Length; i++)
            {
                var decoded = (dynamic)cadenceDecodedAsArray[i];
                
                if(i == 0)
                {
                    Assert.Equal(1726387, decoded.Key);
                    Assert.Equal(true, decoded.Value);
                }
                else if(i == 1)
                {
                    Assert.Equal(Convert.ToInt64(1726387), decoded.Key);
                    Assert.Equal("0x192837123", decoded.Value);
                }
                else if (i == 2)
                {
                    Assert.Equal("ImKey", decoded.Key);
                    Assert.Null(decoded.Value);
                }
            }
        }

        [Fact]
        public void TestCadenceCompositeDecode()
        {
            var compItem = new CadenceCompositeItem
            {
                Id = "id.Foo",
                Fields = new List<CadenceCompositeItemField>
                    {
                        new CadenceCompositeItemField
                        {
                            Name = "name.Bar",
                            Value = new CadenceAddress("0x827612341234")
                        }
                    }
            };

            var cadence = new CadenceComposite(CadenceCompositeType.Resource, compItem);
            var cadenceDecoded = cadence.Decode();

            Assert.Equal(compItem, cadenceDecoded);
        }

        [Fact]
        public void TestCadenceNumberDecode()
        {
            var testItems = new List<CadenceTesting>
            {
                new CadenceTesting
                {
                    Cadence = new CadenceNumber(CadenceNumberType.Int16, "2344"),
                    ExpectedValue = (short)2344
                },
                new CadenceTesting
                {
                    Cadence = new CadenceNumber(CadenceNumberType.Int32, "53213"),
                    ExpectedValue = 53213
                },
                new CadenceTesting
                {
                    Cadence = new CadenceNumber(CadenceNumberType.UInt32, "763212"),
                    ExpectedValue = (uint)763212
                }
            };

            foreach (var item in testItems)
            {
                var decoded = item.Cadence.Decode();
                Assert.Equal(item.ExpectedValue, decoded); // this will check the value and the Type is correct
            }
        }

        public class CadenceTesting
        {
            public ICadence Cadence { get; set; }
            public object ExpectedValue { get; set; }
        }

    }
}
