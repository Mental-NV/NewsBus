using System;

namespace NewsBus.Domain.Models
{
    public class RssFeed
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Uri Url { get; set; }
    }
}