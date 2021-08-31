using System;
using System.Text.Json.Serialization;

namespace NewsBus.Domain.Models
{
    public class Article
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }
        public Uri Url { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTimeOffset PublishDate { get; set; }
    }
}