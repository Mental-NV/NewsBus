using System;
using System.Text.Json.Serialization;

namespace NewsBus.Domain.Models
{
    public class RssFeed
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }
        public string Name { get; set; }
        public Uri Url { get; set; }
    }
}