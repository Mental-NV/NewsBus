using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NewsBus.Application.Interfaces;
using NewsBus.Infrastructure;

namespace NewsBus.WatcherService.Tests
{
    [TestClass]
    public class ArticleIdGeneratorTests : IDisposable
    {
        private readonly ArticleIdGenerator sut;

        public ArticleIdGeneratorTests()
        {
            sut = new ArticleIdGenerator();
        }

        [TestMethod]
        public void Convert_GenerateGuid_NotEmptyGuid()
        {
            // Arrange
            string url = "https://dummyurl";

            // Act
            Guid actual = sut.Convert(url);

            // Assert
            Assert.AreNotEqual(Guid.Empty, actual);
        }

        [TestMethod]
        public void Convert_DifferentUrlsHaveDifferentGuids_GuidsAreUnique()
        {
            string url1 = "https://dummyurl/1";
            string url2 = "https://dummyurl/2";

            // Act
            Guid actual1 = sut.Convert(url1);
            Guid actual2 = sut.Convert(url2);

            // Assert
            Assert.AreNotEqual(actual1, actual2);
        }

        [TestMethod]
        public void Convert_IdenticalUrlsHaveIdenticalGuids_GuidsAreIdentical()
        {
            // Arrange
            string url1 = "https://dummyurl";
            string url2 = "https://dummyurl";

            // Act
            Guid actual = sut.Convert(url1);
            Guid expected = sut.Convert(url2);

            // Assert
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Clean up the resources
        /// </summary>
        public void Dispose()
        {
            sut.Dispose();
        }
    }
}