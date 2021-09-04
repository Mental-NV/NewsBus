using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Threading.Tasks;
using System.Xml;
using NewsBus.Contracts;
using NewsBus.Contracts.Models;

namespace NewsBus.WatcherService.Core
{
    public class RssLoader : IRssLoader
    {
        public Task<IEnumerable<Article>> LoadAsync(Uri rssFeedUrl)
        {
            if (rssFeedUrl is null)
            {
                throw new ArgumentNullException(nameof(rssFeedUrl));
            }

            List<Article> result = new List<Article>();

            using XmlReader reader = XmlReader.Create(rssFeedUrl.ToString());
            SyndicationFeed feed = SyndicationFeed.Load(reader);
            foreach (SyndicationItem item in feed.Items)
            {
                var article = new Article() 
                { 
                    Id = item.Id, 
                    Url = item.Links?.FirstOrDefault()?.Uri, 
                    Title = item.Title?.Text,
                    PublishDate = item.PublishDate,
                    Description = item.Summary?.Text
                };
                result.Add(article);
            }
            return Task.FromResult((IEnumerable<Article>)result);
        }
    }
}