using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NewsBus.Application;
using NewsBus.Application.Interfaces;
using NewsBus.Domain.Models;
using NewsBus.Infrastructure;

namespace NewsBus.WatcherService.Tests
{
    // [TestClass]
    public class RssFeedRepositoryTests
    {
        // [TestMethod]
        public async Task GetItems_GetItemsFromDb_GotValidItems()
        {
            // Arrange
            string cosmosConnectionString = Environment.GetEnvironmentVariable("NEWSBUSCOSMOSDBCONNECTIONSTRING", EnvironmentVariableTarget.Process);
            var target = new RssFeedRepository(cosmosConnectionString, Constants.NewsBusDatabase, Constants.RssFeedsContainer);

            // Act
            IEnumerable<RssFeed> actual = await target.GetItemsAsync();

            // Assert
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