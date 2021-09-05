using System;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NewsBus.Application.Interfaces;
using NewsBus.Domain.Models;
using NewsBus.Infrastructure;

namespace NewsBus.DownloaderService.Tests
{
    /// <summary>
    /// Integration tests for the article repository
    /// </summary>
    [TestClass]
    public class ArticleRepositoryTests
    {
        private readonly string cosmosConnectionString = Environment.GetEnvironmentVariable("NewsBusCosmosDbConnectionString", EnvironmentVariableTarget.Machine);
        const string TestArticleDb = "TestArticleDb";
        const string TestArticleContainer = "TestArticles";
        private readonly ArticleRepository sut;

        public ArticleRepositoryTests()
        {
            sut = new ArticleRepository(cosmosConnectionString, TestArticleDb, TestArticleContainer);
        }

        [TestMethod]
        public async Task PostArticleAsync_PostAndRead_Success()
        {
            // Arrange
            Article expected = GenerateArticle();
            try
            {
                // Act
                bool postResult = await sut.PostArticleAsync(expected);
                Assert.IsTrue(postResult);
                
                // Assert
                Article actual = await sut.GetArticleAsync(expected.Id);
                Assert.IsTrue(AreEqual(expected, actual));
            }
            finally
            {
                // Clean up
                bool deleteResult = await sut.DeleteArticleAsync(expected.Id, expected.Url.ToString());
                Assert.IsTrue(deleteResult);
            }
        }

        [TestMethod]
        public async Task PutArticleAsync_PostPutAndReadV2_Success()
        {
            Article expected = GenerateArticle();
            try
            {
                // Arrange
                bool postResult = await sut.PostArticleAsync(expected);
                Assert.IsTrue(postResult);

                // Act
                expected.Title = "Updated title";
                bool putResult = await sut.PutArticleAsync(expected.Id, expected);
                Assert.IsTrue(putResult);

                // Assert
                Article actual = await sut.GetArticleAsync(expected.Id, expected.Url.ToString());
                Assert.IsTrue(AreEqual(expected, actual));
            }
            finally
            {
                // Clean up
                bool deleteResult = await sut.DeleteArticleAsync(expected.Id, expected.Url.ToString());
                Assert.IsTrue(deleteResult);
            }
        }

        private Article GenerateArticle()
        {
            string id = Guid.NewGuid().ToString();
            return new Article()
            {
                Id = id,
                Url = new Uri($"https://dummyurl/{id}"),
                Title = "Dummy Title",
                Description = "Dummy Description",
                PublishDate = DateTimeOffset.UtcNow
            };
        }

        private bool AreEqual(Article expected, Article actual)
        {
            return expected.Id == actual.Id && expected.Url == actual.Url && expected.Title == actual.Title && expected.Description == actual.Description && expected.PublishDate == actual.PublishDate;
        }
    }
}