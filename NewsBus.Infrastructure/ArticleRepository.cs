using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;
using NewsBus.Application.Interfaces;
using NewsBus.Domain.Models;

namespace NewsBus.Infrastructure
{
    /// <summary>
    /// Represents data access to the article repository
    /// </summary>
    public class ArticleRepository : IArticleRepository
    {
        private readonly CosmosClient client;
        private readonly Database database;
        private readonly Container container;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="cosmosConnectionString">connection string to a consomos db storage</param>
        /// <param name="databaseId">database id</param>
        /// <param name="containerId">container id</param>
        public ArticleRepository(string cosmosConnectionString, string databaseId, string containerId)
        {
            if (string.IsNullOrWhiteSpace(cosmosConnectionString))
            {
                throw new System.ArgumentException($"'{nameof(cosmosConnectionString)}' cannot be null or whitespace.", nameof(cosmosConnectionString));
            }

            if (string.IsNullOrWhiteSpace(databaseId))
            {
                throw new ArgumentException($"'{nameof(databaseId)}' cannot be null or whitespace.", nameof(databaseId));
            }

            if (string.IsNullOrWhiteSpace(containerId))
            {
                throw new ArgumentException($"'{nameof(containerId)}' cannot be null or whitespace.", nameof(containerId));
            }

            client = new CosmosClient(cosmosConnectionString);

            client.CreateDatabaseIfNotExistsAsync(databaseId).GetAwaiter().GetResult();
            database = client.GetDatabase(databaseId);

            database.CreateContainerIfNotExistsAsync(containerId, "/Url").GetAwaiter().GetResult();
            container = database.GetContainer(containerId);
        }

        /// <summary>
        /// Create a new article in the repository
        /// </summary>
        /// <param name="article">the article</param>
        /// <returns>true if success</returns>
        public async Task<bool> PostArticleAsync(Article article)
        {
            if (article is null)
            {
                throw new ArgumentNullException(nameof(article));
            }

            ItemResponse<Article> response = await container.CreateItemAsync(article);
            return response.StatusCode == HttpStatusCode.Created || response.StatusCode == HttpStatusCode.Accepted;
        }

        /// <summary>
        /// Update an article in the repository
        /// </summary>
        /// <param name="id">the article id</param>
        /// <param name="article">the article</param>
        /// <returns>true if success</returns>
        public async Task<bool> PutArticleAsync(string id, Article article)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new ArgumentException($"'{nameof(id)}' cannot be null or whitespace.", nameof(id));
            }

            if (article is null)
            {
                throw new ArgumentNullException(nameof(article));
            }

            ItemResponse<Article> response = await container.ReplaceItemAsync(article, id);
            return response.StatusCode == HttpStatusCode.OK || response.StatusCode == HttpStatusCode.NoContent;
        }

        /// <summary>
        /// Read an article from the repository (slow version)
        /// </summary>
        /// <param name="id">the article id</param>
        /// <returns>the article data model</returns>
        public async Task<Article> GetArticleAsync(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new ArgumentException($"'{nameof(id)}' cannot be null or whitespace.", nameof(id));
            }

            string sqlQuery = $"SELECT * FROM c WHERE c.id = @id OFFSET 0 LIMIT 1";
            QueryDefinition queryDefinition = new QueryDefinition(sqlQuery)
                .WithParameter("@id", id);
            FeedIterator<Article> iterator = container.GetItemQueryIterator<Article>(queryDefinition);
            while (iterator.HasMoreResults)
            {
                FeedResponse<Article> response = await iterator.ReadNextAsync();
                foreach (Article article in response)
                {
                    return article;
                }
            }
            return null;
        }

        /// <summary>
        /// Check if the article is already exist in the repository
        /// </summary>
        /// <param name="id">the article id</param>
        /// <returns>true if the article exists</returns>
        public async Task<bool> Exist(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new ArgumentException($"'{nameof(id)}' cannot be null or whitespace.", nameof(id));
            }

            string sqlQuery = $"SELECT VALUE count(1) FROM c WHERE c.id = @id";
            QueryDefinition queryDefinition = new QueryDefinition(sqlQuery)
                .WithParameter("@id", id);
            FeedIterator<int> iterator = container.GetItemQueryIterator<int>(queryDefinition);
            
            int count = 0;
            while (iterator.HasMoreResults)
            {
                count = (await iterator.ReadNextAsync()).SingleOrDefault();
            }
            return count > 0;
        }

        /// <summary>
        /// Real an article from the repository
        /// </summary>
        /// <param name="id">the article id</param>
        /// <param name="url">the article url (partition key)</param>
        /// <returns>the article data model</returns>
        public async Task<Article> GetArticleAsync(string id, string url)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new ArgumentException($"'{nameof(id)}' cannot be null or whitespace.", nameof(id));
            }

            if (string.IsNullOrWhiteSpace(url))
            {
                throw new ArgumentException($"'{nameof(url)}' cannot be null or whitespace.", nameof(url));
            }

            ItemResponse<Article> response = await container.ReadItemAsync<Article>(id, new PartitionKey(url));
            return response.Resource;
        }

        /// <summary>
        /// Read all articles from the repository
        /// </summary>
        /// <returns>list of articles</returns>
        public async Task<IEnumerable<Article>> GetArticlesAsync()
        {
            List<Article> result = new List<Article>();

            string sqlQuery = $"SELECT * FROM c";
            FeedIterator<Article> iterator = container.GetItemQueryIterator<Article>(sqlQuery);
            while (iterator.HasMoreResults)
            {
                FeedResponse<Article> response = await iterator.ReadNextAsync();
                foreach (Article article in response)
                {
                    result.Add(article);
                }
            }

            return result;
        }

        /// <summary>
        /// Delete an article from the repository
        /// </summary>
        /// <param name="id">the article id</param>
        /// <param name="url">the article url (partition key)</param>
        /// <returns>true if success</returns>
        public async Task<bool> DeleteArticleAsync(string id, string url)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new ArgumentException($"'{nameof(id)}' cannot be null or whitespace.", nameof(id));
            }

            if (string.IsNullOrWhiteSpace(url))
            {
                throw new ArgumentException($"'{nameof(url)}' cannot be null or whitespace.", nameof(url));
            }

            ItemResponse<Article> response = await container.DeleteItemAsync<Article>(id, new PartitionKey(url));
            return response.StatusCode == HttpStatusCode.OK || response.StatusCode == HttpStatusCode.NoContent;
        }
    }
}