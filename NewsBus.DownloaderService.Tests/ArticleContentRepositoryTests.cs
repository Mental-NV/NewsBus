using System;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NewsBus.DownloaderService.Core;

namespace NewsBus.DownloaderService.Tests
{
    [TestClass]
    public class ArticleContentRepositoryTests
    {
        static readonly string storageConnectionString = Environment.GetEnvironmentVariable("NewsBusStorageConnetionString", EnvironmentVariableTarget.Machine);

        [TestMethod]
        public async Task PostGetDelete_CreateReadAndDeleteTheBlob_AllSuccessful()
        {
            ArticleContentRepository target = new ArticleContentRepository(storageConnectionString, "integrationtests");
            string id = Guid.NewGuid().ToString();
            string content = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.";
            try
            {
                bool postResult = await target.PostContentAsync(id, content);
                Assert.IsTrue(postResult);
                string actualContent = await target.GetContentAsync(id);
                Assert.AreEqual(content, actualContent);
                bool deleteResult = await target.DeleteContentAsync(id);
                Assert.IsTrue(deleteResult);
            }
            finally
            {
                await target.DeleteContentAsync(id);
            }
        }
    }
}