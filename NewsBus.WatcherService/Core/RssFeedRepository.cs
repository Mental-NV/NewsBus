using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;
using NewsBus.Domain;
using NewsBus.Domain.Models;

namespace NewsBus.WatcherService.Core
{
    public class RssFeedRepository : IRssFeedRepository
    {
        private readonly string cosmosDbConnectionString;
        private readonly Container container;

        public RssFeedRepository(string cosmosDbConnectionString, string databaseId, string containerId)
        {
            if (string.IsNullOrWhiteSpace(cosmosDbConnectionString))
            {
                throw new ArgumentException($"'{nameof(cosmosDbConnectionString)}' cannot be null or whitespace.", nameof(cosmosDbConnectionString));
            }

            this.cosmosDbConnectionString = cosmosDbConnectionString;
            CosmosClient client = new CosmosClient(cosmosDbConnectionString);
            Database database = client.GetDatabase(databaseId);
            container = database.GetContainer(containerId);
        }

        public async Task<IEnumerable<RssFeed>> GetItemsAsync()
        {
            string sqlQuery = "SELECT * FROM c";
            FeedIterator<RssFeed> iterator = container.GetItemQueryIterator<RssFeed>(sqlQuery);

            List<RssFeed> result = new List<RssFeed>();

            while (iterator.HasMoreResults)
            {
                FeedResponse<RssFeed> response = await iterator.ReadNextAsync();
                foreach (RssFeed item in response)
                {
                    result.Add(item);
                }
            }

            return result;
        }
    }
}