using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;
using NewsBus.Domain;
using NewsBus.Domain.Models;

namespace NewsBus.DownloaderService.Core
{
    public class ArticleRepository : IArticleRepository
    {
        const string DataBaseId = "ArticleDb";
        const string ContainerId = "Articles";
        private readonly CosmosClient client;
        private readonly Database database;
        private readonly Container container;
        public ArticleRepository(string cosmosConnectionString)
        {
            if (string.IsNullOrWhiteSpace(cosmosConnectionString))
            {
                throw new System.ArgumentException($"'{nameof(cosmosConnectionString)}' cannot be null or whitespace.", nameof(cosmosConnectionString));
            }
            client = new CosmosClient(cosmosConnectionString);
            database = client.GetDatabase(DataBaseId);
            container = database.GetContainer(ContainerId);
        }

        public async Task<bool> PostArticleAsync(Article article)
        {
            ItemResponse<Article> response = await container.CreateItemAsync(article);
            return response.StatusCode == HttpStatusCode.Created || response.StatusCode == HttpStatusCode.Accepted;
        }

        public async Task<bool> PutArticleAsync(string id, Article article)
        {
            ItemResponse<Article> response = await container.ReplaceItemAsync(article, id);
            return response.StatusCode == HttpStatusCode.OK || response.StatusCode == HttpStatusCode.NoContent;
        }

        public async Task<Article> GetArticleAsync(string id)
        {
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
        public async Task<Article> GetArticleAsync(string id, string url)
        {
            ItemResponse<Article> response = await container.ReadItemAsync<Article>(id, new PartitionKey(url));
            return response.Resource;
        }
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

        public async Task<bool> DeleteArticleAsync(string id, string url)
        {
            ItemResponse<Article> response = await container.DeleteItemAsync<Article>(id, new PartitionKey(url));
            return response.StatusCode == HttpStatusCode.OK || response.StatusCode == HttpStatusCode.NoContent;
        }
    }
}