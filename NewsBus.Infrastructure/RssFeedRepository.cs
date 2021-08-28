using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NewsBus.Domain;
using NewsBus.Domain.Models;

namespace NewsBus.Infrastructure
{
    public class RssFeedRepository : IRssFeedRepository
    {
        public async Task<IEnumerable<RssFeed>> GetItemsAsync()
        {
            var sampleItem = new RssFeed() { Id = 1, Name = "habr.com", Url = new Uri("https://habr.com/en/rss/all/all") };
            return await Task.FromResult(new [] { sampleItem });
        }
    }
}