using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.ServiceModel.Syndication;
using System.Threading.Tasks;
using System.Xml;
using FluentValidation.Results;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NewsBus.Application.Interfaces;
using NewsBus.Domain.Models;
using NewsBus.Application.Validators;
using NewsBus.Infrastructure;

namespace NewsBus.WatcherService.Tests
{
    [TestClass]
    public class RssLoaderTests
    {
        [TestMethod]
        public async Task LoadAsync_LoadRssFeed_Successfully()
        {
            // Arrange
            IArticleIdGenerator idGenerator = new ArticleIdGenerator();
            IRssLoader rssLoader = new RssLoader(idGenerator);

            // Act
            IEnumerable<Article> result = await rssLoader.LoadAsync(new Uri("https://habr.com/en/rss/all/all"));

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Any());
            ArticleValidator validator = new ArticleValidator();
            foreach (Article article in result)
            {
                ValidationResult valid = validator.Validate(article);
                Assert.IsTrue(valid.IsValid, valid?.Errors?.FirstOrDefault()?.ErrorMessage);
            }
        }
    }
}
