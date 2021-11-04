using ExpectedObjects;
using Flow.Net.Sdk.Cadence;
using Newtonsoft.Json;
using System.Collections.Generic;
using Xunit;

namespace Flow.Net.Sdk.Tests
{
    public class CadenceTest
    {
        private readonly CadenceConverter _cadenceConverter;

        public CadenceTest()
        {
            _cadenceConverter = new CadenceConverter();
        }

        [Fact]
        public void TestCadencePath()
        {
            var testItems = new List<CadenceTestItem>
            {
                new CadenceTestItem
                {
                    Name = "Simple",
                    Cadence = new CadencePath
                        {
                            Value = new CadencePathValue
                            {
                                Domain = "storage",
                                Identifier = "foo"
                            }
                        },
                    ExpectedJson = "{\"type\":\"Path\",\"value\":{\"domain\":\"storage\",\"identifier\":\"foo\"}}"
                },
            };

            TestEncodeAndDecode(testItems);
        }

        [Fact]
        public void TestCadenceCapability()
        {
            var testItems = new List<CadenceTestItem>
            {
                new CadenceTestItem
                {
                    Name = "Simple",
                    Cadence =  new CadenceCapability(new FlowCapabilityValue
                    {
                        Path = new CadencePath
                            {
                                Value = new CadencePathValue
                                {
                                    Domain = "storage",
                                    Identifier = "foo"
                                }
                            },
                        Address = "0x0000000102030405",
                        BorrowType = "Int"

                    }),
                    ExpectedJson = "{\"type\":\"Capability\",\"value\":{\"path\":{\"type\":\"Path\",\"value\":{\"domain\":\"storage\",\"identifier\":\"foo\"}},\"borrowType\":\"Int\",\"address\":\"0x0000000102030405\"}}"
                },
            };

            TestEncodeAndDecode(testItems);
        }

        [Fact]
        public void TestCadenceType()
        {
            var testItems = new List<CadenceTestItem>
            {
                new CadenceTestItem
                {
                    Name = "Simple",
                    Cadence =  new CadenceType(
                        new CadenceTypeValue
                        {
                            StaticType = "Int"
                        }),
                    ExpectedJson = "{\"type\":\"Type\",\"value\":{\"staticType\":\"Int\"}}"
                },
            };

            TestEncodeAndDecode(testItems);
        }

        [Fact]
        public void TestCadenceLink()
        {
            var testItems = new List<CadenceTestItem>
            {
                new CadenceTestItem
                {
                    Name = "Simple",
                    Cadence =  new CadenceLink(
                        new CadenceLinkValue
                        {
                            TargetPath = new CadencePath
                            {
                                Value = new CadencePathValue
                                {
                                    Domain = "storage",
                                    Identifier = "foo"
                                }
                            },
                            BorrowType = "Bar"
                        }),
                    ExpectedJson = "{\"type\":\"Link\",\"value\":{\"targetPath\":{\"type\":\"Path\",\"value\":{\"domain\":\"storage\",\"identifier\":\"foo\"}},\"borrowType\":\"Bar\"}}"
                },
            };

            TestEncodeAndDecode(testItems);
        }

        [Fact]
        public void TestCadenceContract()
        {
            var testItems = new List<CadenceTestItem>
            {
                new CadenceTestItem
                {
                    Name = "Simple",
                    Cadence =  new CadenceComposite(
                        CadenceCompositeType.Contract,
                        new CadenceCompositeItem
                        {
                            Id = "S.test.FooContract",
                            Fields = new List<CadenceCompositeItemValue>
                            {
                                new CadenceCompositeItemValue()
                                {
                                    Name = "a",
                                    Value = new CadenceNumber(CadenceNumberType.Int, "1")
                                },
                                new CadenceCompositeItemValue()
                                {
                                    Name = "b",
                                    Value = new CadenceString("foo")
                                },
                            }
                        }),
                    ExpectedJson = "{\"type\":\"Contract\",\"value\":{\"id\":\"S.test.FooContract\",\"fields\":[{\"name\":\"a\",\"value\":{\"type\":\"Int\",\"value\":\"1\"}},{\"name\":\"b\",\"value\":{\"type\":\"String\",\"value\":\"foo\"}}]}}"
                },
            };

            TestEncodeAndDecode(testItems);
        }

        [Fact]
        public void TestCadenceEvent()
        {
            var testItems = new List<CadenceTestItem>
            {
                new CadenceTestItem
                {
                    Name = "Simple",
                    Cadence =  new CadenceComposite(
                        CadenceCompositeType.Event,
                        new CadenceCompositeItem
                        {
                            Id = "S.test.FooEvent",
                            Fields = new List<CadenceCompositeItemValue>
                            {
                                new CadenceCompositeItemValue()
                                {
                                    Name = "a",
                                    Value = new CadenceNumber(CadenceNumberType.Int, "1")
                                },
                                new CadenceCompositeItemValue()
                                {
                                    Name = "b",
                                    Value = new CadenceString("foo")
                                },
                            }
                        }),
                    ExpectedJson = "{\"type\":\"Event\",\"value\":{\"id\":\"S.test.FooEvent\",\"fields\":[{\"name\":\"a\",\"value\":{\"type\":\"Int\",\"value\":\"1\"}},{\"name\":\"b\",\"value\":{\"type\":\"String\",\"value\":\"foo\"}}]}}"
                },
            };

            TestEncodeAndDecode(testItems);
        }

        [Fact]
        public void TestCadenceStruct()
        {
            var testItems = new List<CadenceTestItem>
            {
                new CadenceTestItem
                {
                    Name = "Simple",
                    Cadence =  new CadenceComposite(
                        CadenceCompositeType.Struct,
                        new CadenceCompositeItem
                        {
                            Id = "S.test.FooStruct",
                            Fields = new List<CadenceCompositeItemValue>
                            {
                                new CadenceCompositeItemValue()
                                {
                                    Name = "a",
                                    Value = new CadenceNumber(CadenceNumberType.Int, "1")
                                },
                                new CadenceCompositeItemValue()
                                {
                                    Name = "b",
                                    Value = new CadenceString("foo")
                                },
                            }
                        }),
                    ExpectedJson = "{\"type\":\"Struct\",\"value\":{\"id\":\"S.test.FooStruct\",\"fields\":[{\"name\":\"a\",\"value\":{\"type\":\"Int\",\"value\":\"1\"}},{\"name\":\"b\",\"value\":{\"type\":\"String\",\"value\":\"foo\"}}]}}"
                },
            };

            TestEncodeAndDecode(testItems);
        }

        [Fact]
        public void TestCadenceResource()
        {
            var testItems = new List<CadenceTestItem>
            {
                new CadenceTestItem
                {
                    Name = "Simple",
                    Cadence =  new CadenceComposite(
                        CadenceCompositeType.Resource,
                        new CadenceCompositeItem
                        {
                            Id = "S.test.Foo",
                            Fields = new List<CadenceCompositeItemValue>
                            {
                                new CadenceCompositeItemValue()
                                {
                                    Name = "uuid",
                                    Value = new CadenceNumber(CadenceNumberType.UInt64, "0")
                                },
                                new CadenceCompositeItemValue()
                                {
                                    Name = "bar",
                                    Value = new CadenceNumber(CadenceNumberType.Int, "42")
                                }
                            }
                        }),
                    ExpectedJson = "{\"type\":\"Resource\",\"value\":{\"id\":\"S.test.Foo\",\"fields\":[{\"name\":\"uuid\",\"value\":{\"type\":\"UInt64\",\"value\":\"0\"}},{\"name\":\"bar\",\"value\":{\"type\":\"Int\",\"value\":\"42\"}}]}}"
                },
            };

            TestEncodeAndDecode(testItems);
        }

        [Fact]
        public void TestCadenceDictionary()
        {
            var testItems = new List<CadenceTestItem>
            {
                new CadenceTestItem
                {
                    Name = "Simple",
                    Cadence =  new CadenceDictionary(
                        new List<CadenceDictionaryKeyValue>
                        {
                            new CadenceDictionaryKeyValue
                            {
                                Key = new CadenceString("a"),
                                Value = new CadenceNumber(CadenceNumberType.Int, "1")
                            },
                            new CadenceDictionaryKeyValue
                            {
                                Key = new CadenceString("b"),
                                Value = new CadenceNumber(CadenceNumberType.Int, "2")
                            },
                            new CadenceDictionaryKeyValue
                            {
                                Key = new CadenceString("c"),
                                Value = new CadenceNumber(CadenceNumberType.Int, "3")
                            }
                        }),
                    ExpectedJson = "{\"type\":\"Dictionary\",\"value\":[{\"key\":{\"type\":\"String\",\"value\":\"a\"},\"value\":{\"type\":\"Int\",\"value\":\"1\"}},{\"key\":{\"type\":\"String\",\"value\":\"b\"},\"value\":{\"type\":\"Int\",\"value\":\"2\"}},{\"key\":{\"type\":\"String\",\"value\":\"c\"},\"value\":{\"type\":\"Int\",\"value\":\"3\"}}]}"
                },
                new CadenceTestItem
                {
                    Name = "Simple",
                    Cadence =  new CadenceDictionary(
                        new List<CadenceDictionaryKeyValue>
                        {
                            new CadenceDictionaryKeyValue
                            {
                                Key = new CadenceString("a"),
                                Value = new CadenceComposite(
                                    CadenceCompositeType.Resource,
                                    new CadenceCompositeItem
                                    {
                                        Id = "S.test.Foo",
                                        Fields = new List<CadenceCompositeItemValue>
                                        {
                                            new CadenceCompositeItemValue()
                                            {
                                                Name = "bar",
                                                Value = new CadenceNumber(CadenceNumberType.Int, "1")
                                            }
                                        }
                                    })
                            },
                            new CadenceDictionaryKeyValue
                            {
                                Key = new CadenceString("b"),
                                Value = new CadenceComposite(
                                    CadenceCompositeType.Resource, new CadenceCompositeItem
                                    {
                                        Id = "S.test.Foo",
                                        Fields = new List<CadenceCompositeItemValue>
                                        {
                                            new CadenceCompositeItemValue()
                                            {
                                                Name = "bar",
                                                Value = new CadenceNumber(CadenceNumberType.Int, "2")
                                            }
                                        }
                                    })
                            },
                            new CadenceDictionaryKeyValue
                            {
                                Key = new CadenceString("c"),
                                Value = new CadenceComposite(
                                    CadenceCompositeType.Resource, new CadenceCompositeItem
                                    {
                                        Id = "S.test.Foo",
                                        Fields = new List<CadenceCompositeItemValue>
                                        {
                                            new CadenceCompositeItemValue()
                                            {
                                                Name = "bar",
                                                Value = new CadenceNumber(CadenceNumberType.Int, "3")
                                            }
                                        }
                                    })
                            }
                        }),
                    ExpectedJson = "{\"type\":\"Dictionary\",\"value\":[{\"key\":{\"type\":\"String\",\"value\":\"a\"},\"value\":{\"type\":\"Resource\",\"value\":{\"id\":\"S.test.Foo\",\"fields\":[{\"name\":\"bar\",\"value\":{\"type\":\"Int\",\"value\":\"1\"}}]}}},{\"key\":{\"type\":\"String\",\"value\":\"b\"},\"value\":{\"type\":\"Resource\",\"value\":{\"id\":\"S.test.Foo\",\"fields\":[{\"name\":\"bar\",\"value\":{\"type\":\"Int\",\"value\":\"2\"}}]}}},{\"key\":{\"type\":\"String\",\"value\":\"c\"},\"value\":{\"type\":\"Resource\",\"value\":{\"id\":\"S.test.Foo\",\"fields\":[{\"name\":\"bar\",\"value\":{\"type\":\"Int\",\"value\":\"3\"}}]}}}]}"
                }
            };

            TestEncodeAndDecode(testItems);
        }

        [Fact]
        public void TestCadenceArray()
        {
            var testItems = new List<CadenceTestItem>
            {
                new CadenceTestItem
                {
                    Name = "Empty",
                    Cadence =  new CadenceArray(),
                    ExpectedJson = "{\"type\":\"Array\",\"value\":[]}"
                },
                new CadenceTestItem
                {
                    Name = "Integers",
                    Cadence =  new CadenceArray(
                        new List<ICadence>
                        {
                            new CadenceNumber(CadenceNumberType.Int, "1"),
                            new CadenceNumber(CadenceNumberType.Int, "2"),
                            new CadenceNumber(CadenceNumberType.Int, "3")
                        }),
                    ExpectedJson = "{\"type\":\"Array\",\"value\":[{\"type\":\"Int\",\"value\":\"1\"},{\"type\":\"Int\",\"value\":\"2\"},{\"type\":\"Int\",\"value\":\"3\"}]}"
                },
                new CadenceTestItem
                {
                    Name = "Resources",
                    Cadence =  new CadenceArray(
                        new List<ICadence>
                        {
                            new CadenceComposite(
                                CadenceCompositeType.Resource, new CadenceCompositeItem
                                {
                                    Id = "S.test.Foo",
                                    Fields = new List<CadenceCompositeItemValue>
                                    {
                                        new CadenceCompositeItemValue()
                                        {
                                            Name = "bar",
                                            Value = new CadenceNumber(CadenceNumberType.Int, "1")
                                        }
                                    }
                                }),
                            new CadenceComposite(
                                CadenceCompositeType.Resource, new CadenceCompositeItem
                                {
                                    Id = "S.test.Foo",
                                    Fields = new List<CadenceCompositeItemValue>
                                    {
                                        new CadenceCompositeItemValue()
                                        {
                                            Name = "bar",
                                            Value = new CadenceNumber(CadenceNumberType.Int, "2")
                                        }
                                    }
                                }),
                            new CadenceComposite(
                                CadenceCompositeType.Resource, new CadenceCompositeItem
                                {
                                    Id = "S.test.Foo",
                                    Fields = new List<CadenceCompositeItemValue>
                                    {
                                        new CadenceCompositeItemValue()
                                        {
                                            Name = "bar",
                                            Value = new CadenceNumber(CadenceNumberType.Int, "3")
                                        }
                                    }
                                })
                        }),
                    ExpectedJson = "{\"type\":\"Array\",\"value\":[{\"type\":\"Resource\",\"value\":{\"id\":\"S.test.Foo\",\"fields\":[{\"name\":\"bar\",\"value\":{\"type\":\"Int\",\"value\":\"1\"}}]}},{\"type\":\"Resource\",\"value\":{\"id\":\"S.test.Foo\",\"fields\":[{\"name\":\"bar\",\"value\":{\"type\":\"Int\",\"value\":\"2\"}}]}},{\"type\":\"Resource\",\"value\":{\"id\":\"S.test.Foo\",\"fields\":[{\"name\":\"bar\",\"value\":{\"type\":\"Int\",\"value\":\"3\"}}]}}]}"
                }
            };

            TestEncodeAndDecode(testItems);
        }

        [Fact]
        public void TestCadenceNumberInt()
        {
            var testItems = new List<CadenceTestItem>
            {
                new CadenceTestItem
                {
                    Name = "Negative",
                    Cadence =  new CadenceNumber(CadenceNumberType.Int, "-42"),
                    ExpectedJson = "{\"type\":\"Int\",\"value\":\"-42\"}"
                },
                new CadenceTestItem
                {
                    Name = "Zero",
                    Cadence =  new CadenceNumber(CadenceNumberType.Int, "0"),
                    ExpectedJson = "{\"type\":\"Int\",\"value\":\"0\"}"
                },
                new CadenceTestItem
                {
                    Name = "Positive",
                    Cadence =  new CadenceNumber(CadenceNumberType.Int, "42"),
                    ExpectedJson = "{\"type\":\"Int\",\"value\":\"42\"}"
                }
            };

            TestEncodeAndDecode(testItems);
        }

        [Fact]
        public void TestCadenceNumberInt8()
        {
            var testItems = new List<CadenceTestItem>
            {
                new CadenceTestItem
                {
                    Name = "Min",
                    Cadence =  new CadenceNumber(CadenceNumberType.Int8, "-128"),
                    ExpectedJson = "{\"type\":\"Int8\",\"value\":\"-128\"}"
                },
                new CadenceTestItem
                {
                    Name = "Zero",
                    Cadence =  new CadenceNumber(CadenceNumberType.Int8, "0"),
                    ExpectedJson = "{\"type\":\"Int8\",\"value\":\"0\"}"
                },
                new CadenceTestItem
                {
                    Name = "Max",
                    Cadence =  new CadenceNumber(CadenceNumberType.Int8, "127"),
                    ExpectedJson = "{\"type\":\"Int8\",\"value\":\"127\"}"
                }
            };

            TestEncodeAndDecode(testItems);
        }

        [Fact]
        public void TestCadenceNumberInt16()
        {
            var testItems = new List<CadenceTestItem>
            {
                new CadenceTestItem
                {
                    Name = "Min",
                    Cadence =  new CadenceNumber(CadenceNumberType.Int16, "-32768"),
                    ExpectedJson = "{\"type\":\"Int16\",\"value\":\"-32768\"}"
                },
                new CadenceTestItem
                {
                    Name = "Zero",
                    Cadence =  new CadenceNumber(CadenceNumberType.Int16, "0"),
                    ExpectedJson = "{\"type\":\"Int16\",\"value\":\"0\"}"
                },
                new CadenceTestItem
                {
                    Name = "Max",
                    Cadence =  new CadenceNumber(CadenceNumberType.Int16, "32767"),
                    ExpectedJson = "{\"type\":\"Int16\",\"value\":\"32767\"}"
                }
            };

            TestEncodeAndDecode(testItems);
        }

        [Fact]
        public void TestCadenceNumberInt32()
        {
            var testItems = new List<CadenceTestItem>
            {
                new CadenceTestItem
                {
                    Name = "Min",
                    Cadence =  new CadenceNumber(CadenceNumberType.Int32, "-2147483648"),
                    ExpectedJson = "{\"type\":\"Int32\",\"value\":\"-2147483648\"}"
                },
                new CadenceTestItem
                {
                    Name = "Zero",
                    Cadence =  new CadenceNumber(CadenceNumberType.Int32, "0"),
                    ExpectedJson = "{\"type\":\"Int32\",\"value\":\"0\"}"
                },
                new CadenceTestItem
                {
                    Name = "Max",
                    Cadence =  new CadenceNumber(CadenceNumberType.Int32, "2147483647"),
                    ExpectedJson = "{\"type\":\"Int32\",\"value\":\"2147483647\"}"
                }
            };

            TestEncodeAndDecode(testItems);
        }

        [Fact]
        public void TestCadenceNumberInt64()
        {
            var testItems = new List<CadenceTestItem>
            {
                new CadenceTestItem
                {
                    Name = "Min",
                    Cadence =  new CadenceNumber(CadenceNumberType.Int64, "-9223372036854775808"),
                    ExpectedJson = "{\"type\":\"Int64\",\"value\":\"-9223372036854775808\"}"
                },
                new CadenceTestItem
                {
                    Name = "Zero",
                    Cadence =  new CadenceNumber(CadenceNumberType.Int64, "0"),
                    ExpectedJson = "{\"type\":\"Int64\",\"value\":\"0\"}"
                },
                new CadenceTestItem
                {
                    Name = "Max",
                    Cadence =  new CadenceNumber(CadenceNumberType.Int64, "9223372036854775807"),
                    ExpectedJson = "{\"type\":\"Int64\",\"value\":\"9223372036854775807\"}"
                }
            };

            TestEncodeAndDecode(testItems);
        }

        [Fact]
        public void TestCadenceNumberInt128()
        {
            var testItems = new List<CadenceTestItem>
            {
                new CadenceTestItem
                {
                    Name = "Min",
                    Cadence =  new CadenceNumber(CadenceNumberType.Int128, "-170141183460469231731687303715884105728"),
                    ExpectedJson = "{\"type\":\"Int128\",\"value\":\"-170141183460469231731687303715884105728\"}"
                },
                new CadenceTestItem
                {
                    Name = "Zero",
                    Cadence =  new CadenceNumber(CadenceNumberType.Int128, "0"),
                    ExpectedJson = "{\"type\":\"Int128\",\"value\":\"0\"}"
                },
                new CadenceTestItem
                {
                    Name = "Max",
                    Cadence =  new CadenceNumber(CadenceNumberType.Int128, "170141183460469231731687303715884105727"),
                    ExpectedJson = "{\"type\":\"Int128\",\"value\":\"170141183460469231731687303715884105727\"}"
                }
            };

            TestEncodeAndDecode(testItems);
        }

        [Fact]
        public void TestCadenceNumberInt256()
        {
            var testItems = new List<CadenceTestItem>
            {
                new CadenceTestItem
                {
                    Name = "Min",
                    Cadence =  new CadenceNumber(CadenceNumberType.Int256, "-57896044618658097711785492504343953926634992332820282019728792003956564819968"),
                    ExpectedJson = "{\"type\":\"Int256\",\"value\":\"-57896044618658097711785492504343953926634992332820282019728792003956564819968\"}"
                },
                new CadenceTestItem
                {
                    Name = "Zero",
                    Cadence =  new CadenceNumber(CadenceNumberType.Int256, "0"),
                    ExpectedJson = "{\"type\":\"Int256\",\"value\":\"0\"}"
                },
                new CadenceTestItem
                {
                    Name = "Max",
                    Cadence =  new CadenceNumber(CadenceNumberType.Int256, "57896044618658097711785492504343953926634992332820282019728792003956564819967"),
                    ExpectedJson = "{\"type\":\"Int256\",\"value\":\"57896044618658097711785492504343953926634992332820282019728792003956564819967\"}"
                }
            };

            TestEncodeAndDecode(testItems);
        }

        [Fact]
        public void TestCadenceAddress()
        {
            var testItems = new List<CadenceTestItem>
            {
                new CadenceTestItem
                {
                    Name = "Address",
                    Cadence =  new CadenceAddress("0x0000000102030405"),
                    ExpectedJson = "{\"type\":\"Address\",\"value\":\"0x0000000102030405\"}"
                },
            };

            TestEncodeAndDecode(testItems);
        }

        [Fact]
        public void TestCadenceString()
        {
            var testItems = new List<CadenceTestItem>
            {
                new CadenceTestItem
                {
                    Name = "Empty",
                    Cadence =  new CadenceString(""),
                    ExpectedJson = "{\"type\":\"String\",\"value\":\"\"}"
                },
                new CadenceTestItem
                {
                    Name = "Non-empty",
                    Cadence =  new CadenceString("foo"),
                    ExpectedJson = "{\"type\":\"String\",\"value\":\"foo\"}"
                }
            };

            TestEncodeAndDecode(testItems);
        }

        [Fact]
        public void TestCadenceBool()
        {
            var testItems = new List<CadenceTestItem>
            {
                new CadenceTestItem
                {
                    Name = "True",
                    Cadence =  new CadenceBool(true),
                    ExpectedJson = "{\"type\":\"Bool\",\"value\":true}"
                },
                new CadenceTestItem
                {
                    Name = "False",
                    Cadence =  new CadenceBool(false),
                    ExpectedJson = "{\"type\":\"Bool\",\"value\":false}"
                }
            };

            TestEncodeAndDecode(testItems);
        }

        [Fact]
        public void TestCadenceOptional()
        {
            var testItems = new List<CadenceTestItem>
            {
                new CadenceTestItem
                {
                    Name = "Nil",
                    Cadence =  new CadenceOptional(),
                    ExpectedJson = "{\"type\":\"Optional\",\"value\":null}"
                },
                new CadenceTestItem
                {
                    Name = "Non-nil",
                    Cadence =  new CadenceOptional(new CadenceNumber(CadenceNumberType.Int, "42")),
                    ExpectedJson = "{\"type\":\"Optional\",\"value\":{\"type\":\"Int\",\"value\":\"42\"}}"
                }
            };

            TestEncodeAndDecode(testItems);
        }

        [Fact]
        public void TestCadenceVoid()
        {
            var testItems = new List<CadenceTestItem>
            {
                new CadenceTestItem
                {
                    Name = "Void",
                    Cadence =  new CadenceVoid(),
                    ExpectedJson = "{\"type\":\"Void\"}"
                }
            };

            TestEncodeAndDecode(testItems);
        }

        private void TestEncodeAndDecode(IEnumerable<CadenceTestItem> testItems)
        {
            foreach (var testItem in testItems)
            {
                var encodeResult = TestEncode(testItem.Cadence, testItem.ExpectedJson);
                TestDecode(encodeResult, testItem.Cadence);
            }
        }

        private static string TestEncode(ICadence cadence, string expectedJson)
        {
            var encoded = cadence.Encode(cadence);
            Assert.Equal(expectedJson, encoded);
            return encoded;
        }

        private void TestDecode(string actualJSON, ICadence cadence)
        {
            var decoded = JsonConvert.DeserializeObject<ICadence>(actualJSON, _cadenceConverter);
            cadence.ToExpectedObject().ShouldEqual(decoded);
        }

        public class CadenceTestItem
        {
            public string Name { get; set; }
            public ICadence Cadence { get; set; }
            public string ExpectedJson { get; set; }
        }
    }
}
