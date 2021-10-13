using Flow.Net.Sdk.Cadence;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Flow.Net.Sdk.Models.EventType
{
    public class AccountCreated
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("value")]
        public AccountCreatedItem Value { get; set; }
    }

    public class AccountCreatedItem
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("fields")]
        public IEnumerable<AccountCreatedItemField> Fields { get; set; }
    }

    public class AccountCreatedItemField
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("value")]
        public CadenceAddress Value { get; set; }
    }
}