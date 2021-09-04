namespace NewsBus.Contracts
{
    public static class Constants
    {
        /// <summary>
        /// Cosmos DB database id
        /// </summary>
        public const string NewsBusDatabase = "NewsBusDb";

        /// <summary>
        /// Cosmos DB container id for RSS feeds
        /// </summary>
        public const string RssFeedsContainer = "RssFeeds";

        /// <summary>
        /// Service Bus queue name for articles to download
        /// </summary>
        public const string DownloadQueue = "downloadqueue";

        /// <summary>
        /// Azure Blob Storage container name for article content
        /// </summary>
        public const string ArticleBlobs = "articleblobs";

        /// <summary>
        /// Cosmos DB container id for articles
        /// </summary>
        public const string ArticlesContainer = "Articles";
    }
}