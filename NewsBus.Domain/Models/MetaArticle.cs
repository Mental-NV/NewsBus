using System;

namespace NewsBus.Domain.Models
{
    public class MetaArticle
    {
        public string ArticleId { get; set; }
        public Uri Url { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTimeOffset PublishDate { get; set; }
    }
}