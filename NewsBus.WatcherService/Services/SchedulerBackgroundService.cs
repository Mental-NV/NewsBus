using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using NewsBus.Domain;
using NewsBus.Domain.Models;

namespace NewsBus.WatcherService.Services
{
    public class SchedulerBackgroundService : BackgroundService
    {
        private readonly IRssFeedRepository rssFeedRepository;
        private readonly IRssLoader rssLoader;
        private readonly IDownloadQueueSender downloadQueueSender;

        public SchedulerBackgroundService(IRssFeedRepository rssFeedRepository,
                                          IRssLoader rssLoader,
                                          IDownloadQueueSender downloadQueueSender)
        {
            this.rssFeedRepository = rssFeedRepository ?? throw new ArgumentNullException(nameof(rssFeedRepository));
            this.rssLoader = rssLoader ?? throw new ArgumentNullException(nameof(rssLoader));
            this.downloadQueueSender = downloadQueueSender ?? throw new ArgumentNullException(nameof(downloadQueueSender));
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                IEnumerable<RssFeed> rssFeeds = await rssFeedRepository.GetItemsAsync();
                foreach (RssFeed feed in rssFeeds)
                {
                    IEnumerable<MetaArticle> articles = await rssLoader.LoadAsync(feed.Url);
                    foreach (MetaArticle article in articles)
                    {
                        await downloadQueueSender.SendAsync(article);
                    }
                }
                await Task.Delay(TimeSpan.FromMinutes(15));
            }
        }
    }
}