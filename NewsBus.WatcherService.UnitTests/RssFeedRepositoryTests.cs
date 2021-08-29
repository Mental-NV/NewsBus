using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NewsBus.Domain.Models;
using NewsBus.WatcherService.Core;

namespace NewsBus.WatcherService.UnitTests
{
    [TestClass]
    public class RssFeedRepositoryTests
    {
        [TestMethod]
        public async Task GetItems_GetItems_Successfully()
        {
            string cosmosConnectionString = Environment.GetEnvironmentVariable("NewsBusCosmosDbConnectionString", EnvironmentVariableTarget.Machine);
            var target = new RssFeedRepository(cosmosConnectionString);

            IEnumerable<RssFeed> actual = await target.GetItemsAsync();

            Assert.IsNotNull(actual);
            Assert.IsTrue(actual.Any());
            foreach (var item in actual)
            {
                Assert.IsFalse(string.IsNullOrWhiteSpace(item.Id));
                Assert.IsFalse(string.IsNullOrWhiteSpace(item.Name));
                Assert.IsNotNull(item.Url);
            }
        }
    }
}