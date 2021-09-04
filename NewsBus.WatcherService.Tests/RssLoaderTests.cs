using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.ServiceModel.Syndication;
using System.Threading.Tasks;
using System.Xml;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NewsBus.Contracts;
using NewsBus.Contracts.Models;
using NewsBus.WatcherService.Core;

namespace NewsBus.WatcherService.Tests
{
    [TestClass]
    public class RssLoaderTests
    {
        [TestMethod]
        public async Task LoadAsync_LoadRssFeed_Successfully()
        {
            // Arrange
            IRssLoader rssLoader = new RssLoader();

            // Act
            IEnumerable<Article> result = await rssLoader.LoadAsync(new Uri("https://habr.com/en/rss/all/all"));

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Any());
            foreach (Article article in result)
            {
                Assert.IsFalse(string.IsNullOrWhiteSpace(article.Id));
                Assert.IsNotNull(article.Url);
                Assert.IsFalse(string.IsNullOrWhiteSpace(article.Description));
                Assert.IsFalse(string.IsNullOrWhiteSpace(article.Title));
                Assert.IsTrue(article.PublishDate > DateTimeOffset.UnixEpoch);
            }
        }
    }
}
