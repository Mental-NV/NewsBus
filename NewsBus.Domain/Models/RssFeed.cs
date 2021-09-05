using System;
using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace NewsBus.Domain.Models
{
    public class RssFeed
    {
        [JsonProperty(propertyName: "id")]
        [JsonPropertyName("id")]
        public string Id { get; set; }
        public string Name { get; set; }
        public Uri Url { get; set; }
    }
}