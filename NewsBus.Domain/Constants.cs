namespace NewsBus.Domain
{
    public static class Constants
    {
        /// <summary>
        /// Service Bus queue name for articles links to download
        /// </summary>
        public const string DownloadQueue = "downloadqueue";

        /// <summary>
        /// Azure Blob Storage container name for article content
        /// </summary>
        public const string ArticleBlobs = "articleblobs";
        
        /// <summary>
        /// Cosmos DB database id for articles
        /// </summary>
        public const string ArticleDb = "ArticleDb";

        /// <summary>
        /// Cosmos DB container id for articles
        /// </summary>
        public const string ArticleContainer = "Articles";
    }
}