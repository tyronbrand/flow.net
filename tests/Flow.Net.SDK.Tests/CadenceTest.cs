using ExpectedObjects;
using Flow.Net.Sdk.Core.Cadence;
using Flow.Net.Sdk.Core.Cadence.Types;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text.Encodings.Web;
using Xunit;

namespace Flow.Net.Sdk.Tests
{
    public class CadenceTest
    {
        private readonly CadenceConverter _cadenceConverter;
        private readonly CadenceTypeConverter _cadenceTypeConverter;

        public CadenceTest()
        {
            _cadenceConverter = new CadenceConverter();
            _cadenceTypeConverter = new CadenceTypeConverter();
        }

        [Fact]
        public void TestCadencePath()
        {
            var testItems = new List<CadenceTestItem>
            {
                new ()
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
                }
            };

            TestEncodeAndDecode(testItems);
        }

        [Fact]
        public void TestCadenceCapability()
        {
            var testItems = new List<CadenceTestItem>
            {
                new ()
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
                        BorrowType = new CadenceType
                        {
                            Kind = "Int"
                        }

                    }),
                    ExpectedJson = "{\"type\":\"Capability\",\"value\":{\"path\":{\"type\":\"Path\",\"value\":{\"domain\":\"storage\",\"identifier\":\"foo\"}},\"address\":\"0x0000000102030405\",\"borrowType\":{\"kind\":\"Int\"}}}"
                }
            };

            TestEncodeAndDecode(testItems);
        }

        [Fact]
        public void TestCadenceTypeValue()
        {
            var testItems = new List<CadenceTestItem>
            {
                new ()
                {
                    Name = "Simple",
                    Cadence =  new CadenceTypeValue(
                        new CadenceTypeValueValue
                        {
                            StaticType = new CadenceType
                            {
                                Kind = "String"
                            }
                        }),
                    ExpectedJson = "{\"type\":\"Type\",\"value\":{\"staticType\":{\"kind\":\"String\"}}}"
                }
            };

            TestEncodeAndDecode(testItems);
        }

        [Fact]
        public void TestCadenceLink()
        {
            var testItems = new List<CadenceTestItem>
            {
                new ()
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
                }
            };

            TestEncodeAndDecode(testItems);
        }

        [Fact]
        public void TestCadenceContract()
        {
            var testItems = new List<CadenceTestItem>
            {
                new ()
                {
                    Name = "Simple",
                    Cadence =  new CadenceComposite(
                        Core.Cadence.CadenceCompositeType.Contract,
                        new CadenceCompositeItem
                        {
                            Id = "S.test.FooContract",
                            Fields = new List<CadenceCompositeItemValue>
                            {
                                new ()
                                {
                                    Name = "a",
                                    Value = new CadenceNumber(CadenceNumberType.Int, "1")
                                },
                                new ()
                                {
                                    Name = "b",
                                    Value = new CadenceString("foo")
                                }
                            }
                        }),
                    ExpectedJson = "{\"type\":\"Contract\",\"value\":{\"id\":\"S.test.FooContract\",\"fields\":[{\"name\":\"a\",\"value\":{\"type\":\"Int\",\"value\":\"1\"}},{\"name\":\"b\",\"value\":{\"type\":\"String\",\"value\":\"foo\"}}]}}"
                }
            };

            TestEncodeAndDecode(testItems);
        }

        [Fact]
        public void TestCadenceEvent()
        {
            var testItems = new List<CadenceTestItem>
            {
                new ()
                {
                    Name = "Simple",
                    Cadence =  new CadenceComposite(
                        Core.Cadence.CadenceCompositeType.Event,
                        new CadenceCompositeItem
                        {
                            Id = "S.test.FooEvent",
                            Fields = new List<CadenceCompositeItemValue>
                            {
                                new ()
                                {
                                    Name = "a",
                                    Value = new CadenceNumber(CadenceNumberType.Int, "1")
                                },
                                new ()
                                {
                                    Name = "b",
                                    Value = new CadenceString("foo")
                                }
                            }
                        }),
                    ExpectedJson = "{\"type\":\"Event\",\"value\":{\"id\":\"S.test.FooEvent\",\"fields\":[{\"name\":\"a\",\"value\":{\"type\":\"Int\",\"value\":\"1\"}},{\"name\":\"b\",\"value\":{\"type\":\"String\",\"value\":\"foo\"}}]}}"
                }
            };

            TestEncodeAndDecode(testItems);
        }

        [Fact]
        public void TestCadenceStruct()
        {
            var testItems = new List<CadenceTestItem>
            {
                new ()
                {
                    Name = "Simple",
                    Cadence =  new CadenceComposite(
                        Core.Cadence.CadenceCompositeType.Struct,
                        new CadenceCompositeItem
                        {
                            Id = "S.test.FooStruct",
                            Fields = new List<CadenceCompositeItemValue>
                            {
                                new ()
                                {
                                    Name = "a",
                                    Value = new CadenceNumber(CadenceNumberType.Int, "1")
                                },
                                new ()
                                {
                                    Name = "b",
                                    Value = new CadenceString("foo")
                                }
                            }
                        }),
                    ExpectedJson = "{\"type\":\"Struct\",\"value\":{\"id\":\"S.test.FooStruct\",\"fields\":[{\"name\":\"a\",\"value\":{\"type\":\"Int\",\"value\":\"1\"}},{\"name\":\"b\",\"value\":{\"type\":\"String\",\"value\":\"foo\"}}]}}"
                }
            };

            TestEncodeAndDecode(testItems);
        }

        [Fact]
        public void TestCadenceResource()
        {
            var testItems = new List<CadenceTestItem>
            {
                new ()
                {
                    Name = "Simple",
                    Cadence =  new CadenceComposite(
                        Core.Cadence.CadenceCompositeType.Resource,
                        new CadenceCompositeItem
                        {
                            Id = "S.test.Foo",
                            Fields = new List<CadenceCompositeItemValue>
                            {
                                new ()
                                {
                                    Name = "uuid",
                                    Value = new CadenceNumber(CadenceNumberType.UInt64, "0")
                                },
                                new ()
                                {
                                    Name = "bar",
                                    Value = new CadenceNumber(CadenceNumberType.Int, "42")
                                }
                            }
                        }),
                    ExpectedJson = "{\"type\":\"Resource\",\"value\":{\"id\":\"S.test.Foo\",\"fields\":[{\"name\":\"uuid\",\"value\":{\"type\":\"UInt64\",\"value\":\"0\"}},{\"name\":\"bar\",\"value\":{\"type\":\"Int\",\"value\":\"42\"}}]}}"
                }
            };

            TestEncodeAndDecode(testItems);
        }

        [Fact]
        public void TestCadenceDictionary()
        {
            var testItems = new List<CadenceTestItem>
            {
                new ()
                {
                    Name = "Simple",
                    Cadence =  new CadenceDictionary(
                        new List<CadenceDictionaryKeyValue>
                        {
                            new ()
                            {
                                Key = new CadenceString("a"),
                                Value = new CadenceNumber(CadenceNumberType.Int, "1")
                            },
                            new ()
                            {
                                Key = new CadenceString("b"),
                                Value = new CadenceNumber(CadenceNumberType.Int, "2")
                            },
                            new ()
                            {
                                Key = new CadenceString("c"),
                                Value = new CadenceNumber(CadenceNumberType.Int, "3")
                            }
                        }),
                    ExpectedJson = "{\"type\":\"Dictionary\",\"value\":[{\"key\":{\"type\":\"String\",\"value\":\"a\"},\"value\":{\"type\":\"Int\",\"value\":\"1\"}},{\"key\":{\"type\":\"String\",\"value\":\"b\"},\"value\":{\"type\":\"Int\",\"value\":\"2\"}},{\"key\":{\"type\":\"String\",\"value\":\"c\"},\"value\":{\"type\":\"Int\",\"value\":\"3\"}}]}"
                },
                new ()
                {
                    Name = "Simple",
                    Cadence =  new CadenceDictionary(
                        new List<CadenceDictionaryKeyValue>
                        {
                            new ()
                            {
                                Key = new CadenceString("a"),
                                Value = new CadenceComposite(
                                    Core.Cadence.CadenceCompositeType.Resource,
                                    new CadenceCompositeItem
                                    {
                                        Id = "S.test.Foo",
                                        Fields = new List<CadenceCompositeItemValue>
                                        {
                                            new ()
                                            {
                                                Name = "bar",
                                                Value = new CadenceNumber(CadenceNumberType.Int, "1")
                                            }
                                        }
                                    })
                            },
                            new ()
                            {
                                Key = new CadenceString("b"),
                                Value = new CadenceComposite(
                                    Core.Cadence.CadenceCompositeType.Resource, new CadenceCompositeItem
                                    {
                                        Id = "S.test.Foo",
                                        Fields = new List<CadenceCompositeItemValue>
                                        {
                                            new ()
                                            {
                                                Name = "bar",
                                                Value = new CadenceNumber(CadenceNumberType.Int, "2")
                                            }
                                        }
                                    })
                            },
                            new ()
                            {
                                Key = new CadenceString("c"),
                                Value = new CadenceComposite(
                                    Core.Cadence.CadenceCompositeType.Resource, new CadenceCompositeItem
                                    {
                                        Id = "S.test.Foo",
                                        Fields = new List<CadenceCompositeItemValue>
                                        {
                                            new ()
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
                new ()
                {
                    Name = "Empty",
                    Cadence =  new CadenceArray(),
                    ExpectedJson = "{\"type\":\"Array\",\"value\":[]}"
                },
                new ()
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
                new ()
                {
                    Name = "Resources",
                    Cadence =  new CadenceArray(
                        new List<ICadence>
                        {
                            new CadenceComposite(
                                Core.Cadence.CadenceCompositeType.Resource, new CadenceCompositeItem
                                {
                                    Id = "S.test.Foo",
                                    Fields = new List<CadenceCompositeItemValue>
                                    {
                                        new ()
                                        {
                                            Name = "bar",
                                            Value = new CadenceNumber(CadenceNumberType.Int, "1")
                                        }
                                    }
                                }),
                            new CadenceComposite(
                                Core.Cadence.CadenceCompositeType.Resource, new CadenceCompositeItem
                                {
                                    Id = "S.test.Foo",
                                    Fields = new List<CadenceCompositeItemValue>
                                    {
                                        new ()
                                        {
                                            Name = "bar",
                                            Value = new CadenceNumber(CadenceNumberType.Int, "2")
                                        }
                                    }
                                }),
                            new CadenceComposite(
                                Core.Cadence.CadenceCompositeType.Resource, new CadenceCompositeItem
                                {
                                    Id = "S.test.Foo",
                                    Fields = new List<CadenceCompositeItemValue>
                                    {
                                        new ()
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
                new ()
                {
                    Name = "Negative",
                    Cadence =  new CadenceNumber(CadenceNumberType.Int, "-42"),
                    ExpectedJson = "{\"type\":\"Int\",\"value\":\"-42\"}"
                },
                new ()
                {
                    Name = "Zero",
                    Cadence =  new CadenceNumber(CadenceNumberType.Int, "0"),
                    ExpectedJson = "{\"type\":\"Int\",\"value\":\"0\"}"
                },
                new ()
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
                new ()
                {
                    Name = "Min",
                    Cadence =  new CadenceNumber(CadenceNumberType.Int8, "-128"),
                    ExpectedJson = "{\"type\":\"Int8\",\"value\":\"-128\"}"
                },
                new ()
                {
                    Name = "Zero",
                    Cadence =  new CadenceNumber(CadenceNumberType.Int8, "0"),
                    ExpectedJson = "{\"type\":\"Int8\",\"value\":\"0\"}"
                },
                new ()
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
                new ()
                {
                    Name = "Min",
                    Cadence =  new CadenceNumber(CadenceNumberType.Int16, "-32768"),
                    ExpectedJson = "{\"type\":\"Int16\",\"value\":\"-32768\"}"
                },
                new ()
                {
                    Name = "Zero",
                    Cadence =  new CadenceNumber(CadenceNumberType.Int16, "0"),
                    ExpectedJson = "{\"type\":\"Int16\",\"value\":\"0\"}"
                },
                new ()
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
                new ()
                {
                    Name = "Min",
                    Cadence =  new CadenceNumber(CadenceNumberType.Int32, "-2147483648"),
                    ExpectedJson = "{\"type\":\"Int32\",\"value\":\"-2147483648\"}"
                },
                new ()
                {
                    Name = "Zero",
                    Cadence =  new CadenceNumber(CadenceNumberType.Int32, "0"),
                    ExpectedJson = "{\"type\":\"Int32\",\"value\":\"0\"}"
                },
                new ()
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
                new ()
                {
                    Name = "Min",
                    Cadence =  new CadenceNumber(CadenceNumberType.Int64, "-9223372036854775808"),
                    ExpectedJson = "{\"type\":\"Int64\",\"value\":\"-9223372036854775808\"}"
                },
                new()
                {
                    Name = "Zero",
                    Cadence =  new CadenceNumber(CadenceNumberType.Int64, "0"),
                    ExpectedJson = "{\"type\":\"Int64\",\"value\":\"0\"}"
                },
                new()
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
                new()
                {
                    Name = "Min",
                    Cadence =  new CadenceNumber(CadenceNumberType.Int128, "-170141183460469231731687303715884105728"),
                    ExpectedJson = "{\"type\":\"Int128\",\"value\":\"-170141183460469231731687303715884105728\"}"
                },
                new()
                {
                    Name = "Zero",
                    Cadence =  new CadenceNumber(CadenceNumberType.Int128, "0"),
                    ExpectedJson = "{\"type\":\"Int128\",\"value\":\"0\"}"
                },
                new()
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
                new()
                {
                    Name = "Min",
                    Cadence =  new CadenceNumber(CadenceNumberType.Int256, "-57896044618658097711785492504343953926634992332820282019728792003956564819968"),
                    ExpectedJson = "{\"type\":\"Int256\",\"value\":\"-57896044618658097711785492504343953926634992332820282019728792003956564819968\"}"
                },
                new()
                {
                    Name = "Zero",
                    Cadence =  new CadenceNumber(CadenceNumberType.Int256, "0"),
                    ExpectedJson = "{\"type\":\"Int256\",\"value\":\"0\"}"
                },
                new()
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
                new()
                {
                    Name = "Address",
                    Cadence =  new CadenceAddress("0x0000000102030405"),
                    ExpectedJson = "{\"type\":\"Address\",\"value\":\"0x0000000102030405\"}"
                }
            };

            TestEncodeAndDecode(testItems);
        }

        [Fact]
        public void TestCadenceString()
        {
            var testItems = new List<CadenceTestItem>
            {
                new()
                {
                    Name = "Empty",
                    Cadence =  new CadenceString(""),
                    ExpectedJson = "{\"type\":\"String\",\"value\":\"\"}"
                },
                new()
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
                new()
                {
                    Name = "True",
                    Cadence =  new CadenceBool(true),
                    ExpectedJson = "{\"type\":\"Bool\",\"value\":true}"
                },
                new()
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
                new()
                {
                    Name = "Nil",
                    Cadence =  new CadenceOptional(),
                    ExpectedJson = "{\"type\":\"Optional\",\"value\":null}"
                },
                new()
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
                new()
                {
                    Name = "Void",
                    Cadence =  new CadenceVoid(),
                    ExpectedJson = "{\"type\":\"Void\"}"
                }
            };

            TestEncodeAndDecode(testItems);
        }

        [Fact]
        public void CadenceTypes()
        {
            var cadenceTypes = new List<string>
            {
                "Any","AnyStruct","AnyResource","Type","Void","Never","Bool","String","Character",
                "Bytes","Address","Number","SignedNumber","Integer","SignedInteger","FixedPoint",
                "SignedFixedPoint","Int","Int8","Int16","Int32","Int64","Int128","Int256","UInt",
                "UInt8","UInt16","UInt32","UInt64","UInt128","UInt256","Word8","Word16","Word32",
                "Word64","Fix64","UFix64","Path","CapabilityPath","StoragePath","PublicPath",
                "PrivatePath","AuthAccount","PublicAccount","AuthAccount.Keys","PublicAccount.Keys",
                "AuthAccount.Contracts","PublicAccount.Contracts","DeployedContract","AccountKey","Block"
            };

            var testItems = new List<CadenceTestItem>();

            foreach (var type in cadenceTypes)
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

                testItems.Add(new CadenceTestItem
                {
                    Name = $"CadenceTypeValueWith{type}Type",
                    Cadence = new CadenceTypeValue(
                        new CadenceTypeValueValue
                        {
                            StaticType = new CadenceType
                            {
                                Kind = type
                            }
                        }),
                    ExpectedJson = "{\"type\":\"Type\",\"value\":{\"staticType\":{\"kind\":\"" + type + "\"}}}"
                });
            }

            TestEncodeAndDecode(testItems);
        }

        [Fact]
        public void TestCadenceOptionalType()
        {
            var testItems = new List<CadenceTestItem>
            {
                new ()
                {
                    Name = "Simple",
                    Cadence =  new CadenceTypeValue(
                        new CadenceTypeValueValue
                        {
                            StaticType = new CadenceOptionalType
                            {
                                Type = new CadenceType
                                {
                                    Kind = "Int"
                                }
                            }
                        }),
                    ExpectedJson = "{\"type\":\"Type\",\"value\":{\"staticType\":{\"kind\":\"Optional\",\"type\":{\"kind\":\"Int\"}}}}"
                }
            };

            TestEncodeAndDecode(testItems);
        }

        [Fact]
        public void TestCadenceVariableSizedArrayType()
        {
            var testItems = new List<CadenceTestItem>
            {
                new ()
                {
                    Name = "Simple",
                    Cadence =  new CadenceTypeValue(
                        new CadenceTypeValueValue
                        {
                            StaticType = new CadenceVariableSizedArrayType
                            {
                                Type = new CadenceType
                                {
                                    Kind = "Int"
                                }
                            }
                        }),
                    ExpectedJson = "{\"type\":\"Type\",\"value\":{\"staticType\":{\"kind\":\"VariableSizedArray\",\"type\":{\"kind\":\"Int\"}}}}"
                }
            };

            TestEncodeAndDecode(testItems);
        }

        [Fact]
        public void TestCadenceConstantSizedArrayType()
        {
            var testItems = new List<CadenceTestItem>
            {
                new ()
                {
                    Name = "Simple",
                    Cadence =  new CadenceTypeValue(
                        new CadenceTypeValueValue
                        {
                            StaticType = new CadenceConstantSizedArrayType
                            {
                                Type = new CadenceType
                                {
                                    Kind = "Int"
                                },
                                Size = 3
                            }
                        }),
                    ExpectedJson = "{\"type\":\"Type\",\"value\":{\"staticType\":{\"kind\":\"ConstantSizedArray\",\"type\":{\"kind\":\"Int\"},\"size\":3}}}"
                }
            };

            TestEncodeAndDecode(testItems);
        }

        [Fact]
        public void TestCadenceDictionaryType()
        {
            var testItems = new List<CadenceTestItem>
            {
                new ()
                {
                    Name = "Simple",
                    Cadence =  new CadenceTypeValue(
                        new CadenceTypeValueValue
                        {
                            StaticType = new CadenceDictionaryType
                            {
                                Key = new CadenceType
                                {
                                    Kind = "Int"
                                },
                                Value = new CadenceType
                                {
                                    Kind = "String"
                                }
                            }
                        }),
                    ExpectedJson = "{\"type\":\"Type\",\"value\":{\"staticType\":{\"kind\":\"Dictionary\",\"key\":{\"kind\":\"Int\"},\"value\":{\"kind\":\"String\"}}}}"
                }
            };

            TestEncodeAndDecode(testItems);
        }

        [Fact]
        public void TestCadenceComposeiteType()
        {
            var testItems = new List<CadenceTestItem>();

            foreach (CadenceCompositeTypeKind kind in Enum.GetValues(typeof(CadenceCompositeTypeKind)))
            {
                testItems.Add(
                    new()
                    {
                        Name = "Simple",
                        Cadence = new CadenceTypeValue(
                        new CadenceTypeValueValue()
                        {
                            StaticType = new Core.Cadence.Types.CadenceCompositeType(kind)
                            {
                                TypeId = "S.test.S",
                                Initializers = new List<IList<CadenceInitializerType>>
                                {
                                    new List<CadenceInitializerType>
                                    {
                                        new CadenceInitializerType
                                        {
                                            Label = "foo",
                                            Id = "bar",
                                            Type = new CadenceType
                                            {
                                                Kind = "Int"
                                            }
                                        }
                                    },
                                    new List<CadenceInitializerType>
                                    {
                                        new CadenceInitializerType
                                        {
                                            Label = "qux",
                                            Id = "baz",
                                            Type = new CadenceType
                                            {
                                                Kind = "String"
                                            }
                                        }
                                    }
                                },
                                Fields = new List<CadenceFieldType>
                                {
                                    new CadenceFieldType
                                    {
                                        Id = "foo",
                                        Type = new CadenceType
                                        {
                                            Kind = "Int"
                                        }
                                    }
                                }
                            }
                        }),
                        ExpectedJson = "{\"type\":\"Type\",\"value\":{\"staticType\":{\"kind\":\"" + kind.ToString() + "\",\"type\":\"\",\"typeID\":\"S.test.S\",\"initializers\":[[{\"label\":\"foo\",\"id\":\"bar\",\"type\":{\"kind\":\"Int\"}}],[{\"label\":\"qux\",\"id\":\"baz\",\"type\":{\"kind\":\"String\"}}]],\"fields\":[{\"id\":\"foo\",\"type\":{\"kind\":\"Int\"}}]}}}"
                    }
                );
            }

            TestEncodeAndDecode(testItems);
        }

        [Fact]
        public void TestCadenceEnumType()
        {
            var testItems = new List<CadenceTestItem>
            {
                new()
                {
                    Name = "Simple",
                    Cadence = new CadenceTypeValue(
                    new CadenceTypeValueValue()
                    {
                        StaticType = new CadenceEnumType
                        {
                            Type = new CadenceType
                            {
                                Kind = "String"
                            },
                            TypeId = "S.test.S",
                            Fields = new List<CadenceFieldType>
                            {
                                new CadenceFieldType
                                {
                                    Id = "foo",
                                    Type = new CadenceType
                                    {
                                        Kind = "Int"
                                    }
                                }
                            }
                        }
                    }),
                    ExpectedJson = "{\"type\":\"Type\",\"value\":{\"staticType\":{\"kind\":\"Enum\",\"type\":{\"kind\":\"String\"},\"typeID\":\"S.test.S\",\"initializers\":[],\"fields\":[{\"id\":\"foo\",\"type\":{\"kind\":\"Int\"}}]}}}"
                }
            };


            TestEncodeAndDecode(testItems);
        }

        [Fact]
        public void TestCadenceReferenceType()
        {
            var testItems = new List<CadenceTestItem>
            {
                new ()
                {
                    Name = "Simple",
                    Cadence =  new CadenceTypeValue
                    {
                        Value = new CadenceTypeValueValue
                        {
                            StaticType = new CadenceReferenceType
                            {
                                Authorized = false,
                                Type = new CadenceType
                                {
                                    Kind = "Int"
                                }
                            }
                        }
                    },
                    ExpectedJson = "{\"type\":\"Type\",\"value\":{\"staticType\":{\"kind\":\"Reference\",\"authorized\":false,\"type\":{\"kind\":\"Int\"}}}}"
                }
            };

            TestEncodeAndDecode(testItems);
        }

        [Fact]
        public void TestCadenceFunctionType()
        {
            var testItems = new List<CadenceTestItem>
            {
                new ()
                {
                    Name = "Simple",
                    Cadence =  new CadenceTypeValue
                    {
                        Value = new CadenceTypeValueValue
                        {
                            StaticType = new CadenceFunctionType
                            {
                                TypeId = "Foo",
                                Parameters = new List<CadenceParameterType>
                                {
                                    new CadenceParameterType
                                    {
                                        Label = "qux",
                                        Id = "baz",
                                        Type = new CadenceType
                                        {
                                            Kind = "String"
                                        }
                                    }
                                },
                                Return = new CadenceType
                                {
                                    Kind = "Int"
                                }
                            }
                        }
                    },
                    ExpectedJson = "{\"type\":\"Type\",\"value\":{\"staticType\":{\"kind\":\"Function\",\"typeID\":\"Foo\",\"parameters\":[{\"label\":\"qux\",\"id\":\"baz\",\"type\":{\"kind\":\"String\"}}],\"return\":{\"kind\":\"Int\"}}}}"
                }
            };

            TestEncodeAndDecode(testItems);
        }

        [Fact]
        public void TestCadenceCapabilityType()
        {
            var testItems = new List<CadenceTestItem>
            {
                new ()
                {
                    Name = "Simple",
                    Cadence =  new CadenceTypeValue
                    {
                        Value = new CadenceTypeValueValue
                        {
                            StaticType = new CadenceCapabilityType
                            {
                                Type = new CadenceType
                                {
                                    Kind = "Int"
                                }
                            }
                        }
                    },
                    ExpectedJson = "{\"type\":\"Type\",\"value\":{\"staticType\":{\"kind\":\"Capability\",\"type\":{\"kind\":\"Int\"}}}}"
                }
            };

            TestEncodeAndDecode(testItems);
        }

        [Fact]
        public void TestCadenceRestrictedType()
        {
            var testItems = new List<CadenceTestItem>
            {
                new ()
                {
                    Name = "Simple",
                    Cadence =  new CadenceTypeValue
                    {
                        Value = new CadenceTypeValueValue
                        {
                            StaticType = new CadenceRestrictedType
                            {
                                TypeId = "Foo",
                                Type = new CadenceType
                                {
                                    Kind = "Int"
                                },
                                Restrictions = new List<ICadenceType>
                                {
                                    new CadenceType
                                    {
                                        Kind = "String"
                                    }
                                }
                            }
                        }
                    },
                    ExpectedJson = "{\"type\":\"Type\",\"value\":{\"staticType\":{\"kind\":\"Restriction\",\"typeID\":\"Foo\",\"type\":{\"kind\":\"Int\"},\"restrictions\":[{\"kind\":\"String\"}]}}}"
                }
            };

            TestEncodeAndDecode(testItems);
        }

        [Fact]
        public void CadenceRepeatedTypes()
        {
            var resource = new Core.Cadence.Types.CadenceCompositeType(CadenceCompositeTypeKind.Resource)
            {
                TypeId = "S.test.Foo",
                Initializers = new List<IList<CadenceInitializerType>>(),
                Fields = new List<CadenceFieldType>
                {
                    new CadenceFieldType
                    {
                        Id = "foo",
                        Type = new CadenceOptionalType{
                            Type = new Core.Cadence.Types.CadenceCompositeType(CadenceCompositeTypeKind.Resource)
                            {
                                TypeId = "S.test.Foo",
                                Fields = new List<CadenceFieldType>
                                {
                                    new CadenceFieldType
                                    {
                                        Id = "foo"
                                    }
                                }
                            }
                        }
                    }
                }
            };

            var testItems = new List<CadenceTestItem>
            {
                new()
                    {
                        Name = "Simple",
                        Cadence = new CadenceTypeValue(
                        new CadenceTypeValueValue()
                        {
                            StaticType = resource
                        }),
                        ExpectedJson = "{\"type\":\"Type\",\"value\":{\"staticType\":{\"kind\":\"Resource\",\"type\":\"\",\"typeID\":\"S.test.Foo\",\"initializers\":[],\"fields\":[{\"id\":\"foo\",\"type\":{\"kind\":\"Optional\",\"type\":\"S.test.Foo\"}}]}}}"
                    }
            };

            TestEncodeAndDecode(testItems);
        }

        [Fact]
        public void CadenceNonRecursiceRepeatedTypes()
        {
            var fooType = new Core.Cadence.Types.CadenceCompositeType(CadenceCompositeTypeKind.Resource)
            {
                TypeId = "S.test.Foo"
            };

            var barType = new Core.Cadence.Types.CadenceCompositeType(CadenceCompositeTypeKind.Resource)
            {
                TypeId = "S.test.Bar",
                Fields = new List<CadenceFieldType>
                {
                    new CadenceFieldType
                    {
                        Id = "foo1",
                        Type = fooType
                    },
                    new CadenceFieldType
                    {
                        Id = "foo2",
                        Type = fooType
                    }
                }
            };

            var testItems = new List<CadenceTestItem>
            {
                new()
                    {
                        Name = "Simple",
                        Cadence = new CadenceTypeValue(
                        new CadenceTypeValueValue()
                        {
                            StaticType = barType
                        }),
                        ExpectedJson = "{\"type\":\"Type\",\"value\":{\"staticType\":{\"kind\":\"Resource\",\"type\":\"\",\"typeID\":\"S.test.Bar\",\"initializers\":[],\"fields\":[{\"id\":\"foo1\",\"type\":{\"kind\":\"Resource\",\"type\":\"\",\"typeID\":\"S.test.Foo\",\"initializers\":[],\"fields\":[]}},{\"id\":\"foo2\",\"type\":\"S.test.Foo\"}]}}}"
                    }
            };

            TestEncodeAndDecode(testItems);
        }

        [Fact]
        public void TestCadenceTypeValueEmpty()
        {
            var testItems = new List<CadenceTestItem>
            {
                new ()
                {
                    Name = "Simple",
                    Cadence =  new CadenceTypeValue
                    {
                        Value = new CadenceTypeValueValue
                        {
                            StaticType = new CadenceTypeValueAsString 
                            {
                                Value = ""
                            }
                        }
                    },
                    ExpectedJson = "{\"type\":\"Type\",\"value\":{\"staticType\":\"\"}}"
                }
            };

            TestEncodeAndDecode(testItems);
        }

        [Fact]
        public void TestCadenceTypeValueComplex()
        {
            var testItems = new List<CadenceTestItem>
            {
                new ()
                {
                    Name = "Simple",
                    Cadence =  new CadenceTypeValue
                    {
                        Value = new CadenceTypeValueValue
                        {
                            StaticType = new Core.Cadence.Types.CadenceCompositeType(CadenceCompositeTypeKind.Resource)
                            {
                                TypeId = "A.321d8fcde05f6e8c.Seussibles.NFT",
                                Fields = new List<CadenceFieldType>
                                {
                                    new CadenceFieldType
                                    {
                                        Id = "uuid",
                                        Type = new CadenceType
                                        {
                                            Kind = "UInt64"
                                        }
                                    },
                                    new CadenceFieldType
                                    {
                                        Id = "id",
                                        Type = new CadenceType
                                        {
                                            Kind = "UInt64"
                                        }
                                    },
                                    new CadenceFieldType
                                    {
                                        Id = "mintNumber",
                                        Type = new CadenceType
                                        {
                                            Kind = "UInt32"
                                        }
                                    },
                                    new CadenceFieldType
                                    {
                                        Id = "contentCapability",
                                        Type = new CadenceCapabilityType
                                        {
                                            Type = new CadenceTypeValueAsString
                                            {
                                                Value = ""
                                            }
                                        }
                                    },
                                    new CadenceFieldType
                                    {
                                        Id = "contentId",
                                        Type = new CadenceType
                                        {
                                            Kind = "String"
                                        }
                                    },
                                }
                            }
                        }
                    },
                    ExpectedJson = "{\"type\":\"Type\",\"value\":{\"staticType\":{\"kind\":\"Resource\",\"type\":\"\",\"typeID\":\"A.321d8fcde05f6e8c.Seussibles.NFT\",\"initializers\":[],\"fields\":[{\"id\":\"uuid\",\"type\":{\"kind\":\"UInt64\"}},{\"id\":\"id\",\"type\":{\"kind\":\"UInt64\"}},{\"id\":\"mintNumber\",\"type\":{\"kind\":\"UInt32\"}},{\"id\":\"contentCapability\",\"type\":{\"kind\":\"Capability\",\"type\":\"\"}},{\"id\":\"contentId\",\"type\":{\"kind\":\"String\"}}]}}}"
                }
            };

            TestEncodeAndDecode(testItems);
        }

        [Fact]
        public void TestCadenceTypeValueAsString()
        {
            var barType = new Core.Cadence.Types.CadenceCompositeType(CadenceCompositeTypeKind.Resource)
            {
                TypeId = "A.2d4c3caffbeab845.FLOAT.FLOATEvent",
                Fields = new List<CadenceFieldType>
                {
                    new CadenceFieldType
                    {
                        Id = "currentHolders",
                        Type = new CadenceDictionaryType
                        {
                            Key = new CadenceType
                            {
                                Kind = "Int"
                            },
                            Value = new CadenceTypeValueAsString
                            {
                                Value = "A.2d4c3caffbeab845.FLOAT.TokenIdentifier"
                            }
                        }
                    }
                }
            };

            var expectedJson = "{\"kind\":\"Resource\",\"type\":\"\",\"typeID\":\"A.2d4c3caffbeab845.FLOAT.FLOATEvent\",\"initializers\":[],\"fields\":[{\"id\":\"currentHolders\",\"type\":{\"kind\":\"Dictionary\",\"key\":{\"kind\":\"Int\"},\"value\":\"A.2d4c3caffbeab845.FLOAT.TokenIdentifier\"}}]}";

            TestEncode(barType, expectedJson);
            TestDecode(expectedJson, barType);
        }


        private void TestEncodeAndDecode(IEnumerable<CadenceTestItem> testItems)
        {
            foreach (var testItem in testItems)
            {
                string encodeResult;
                if (testItem.Cadence != null)
                {
                    encodeResult = TestEncode(testItem.Cadence, testItem.ExpectedJson);
                    TestDecode(encodeResult, testItem.Cadence);
                }
                else
                {
                    encodeResult = TestEncode(testItem.CadenceType, testItem.ExpectedJson);
                    TestDecode(encodeResult, testItem.CadenceType);
                }
            }
        }

        private static string TestEncode(ICadence cadence, string expectedJson)
        {
            var encoded = cadence.Encode(cadence);
            Assert.Equal(expectedJson, encoded);
            return encoded;
        }

        private static string TestEncode(ICadenceType cadenceType, string expectedJson)
        {
            var encoded = cadenceType.Encode(cadenceType);
            Assert.Equal(expectedJson, encoded);
            return encoded;
        }

        private void TestDecode(string actualJson, ICadence cadence)
        {
            var decoded = CadenceExtensions.Decode(actualJson);
            cadence.ToExpectedObject().ShouldEqual(decoded);
        }

        private void TestDecode(string actualJson, ICadenceType cadenceType)
        {
            var decoded = CadenceExtensions.DecodeType(actualJson);
            cadenceType.ToExpectedObject().ShouldEqual(decoded);
        }

        private class CadenceTestItem
        {
            public string Name { get; set; }
            public ICadence Cadence { get; init; }
            public ICadenceType CadenceType { get; init; }
            public string ExpectedJson { get; init; }
        }
    }
}
