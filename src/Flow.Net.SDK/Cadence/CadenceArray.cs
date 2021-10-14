﻿using Newtonsoft.Json;
using System.Collections.Generic;

namespace Flow.Net.Sdk.Cadence
{
    public class CadenceArray : ICadence
    {
        public CadenceArray()
        {
            Value = new List<ICadence>();
        }

        public CadenceArray(IEnumerable<ICadence> value)
        {
            Value = value;
        }

        [JsonProperty("type")]
        public string Type => "Array";

        [JsonProperty("value")]
        public IEnumerable<ICadence> Value { get; set; }

        public object Decode()
        {
            var arrayItems = new List<object>();
            foreach(var item in Value)
                arrayItems.Add(item.Decode());

            return arrayItems.ToArray();
        }
    }
}
