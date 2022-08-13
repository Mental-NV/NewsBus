using System;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NewsBus.Infrastructure;

namespace NewsBus.DownloaderService.Tests
{
    // [TestClass]
    public class ArticleContentRepositoryTests
    {
        private readonly ArticleContentRepository sut;

        public ArticleContentRepositoryTests()
        {
            string storageConnectionString = Environment.GetEnvironmentVariable("NEWSBUSSTORAGECONNETIONSTRING", EnvironmentVariableTarget.Machine);
            sut = new ArticleContentRepository(storageConnectionString, "integrationtests");
        }

        [TestMethod]
        public async Task PostContentAsync_CreateReadAndDeleteTheBlob_AllSuccessful()
        {
            string id = Guid.NewGuid().ToString();
            string content = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.";
            try
            {
                // Arrange
                bool postResult = await sut.PostContentAsync(id, content);
                Assert.IsTrue(postResult);

                // Act
                string actualContent = await sut.GetContentAsync(id);

                // Assert
                Assert.AreEqual(content, actualContent);
            }
            finally
            {
                // Clean up
                bool deleteResult = await sut.DeleteContentAsync(id);
                Assert.IsTrue(deleteResult);
            }
        }
    }
}