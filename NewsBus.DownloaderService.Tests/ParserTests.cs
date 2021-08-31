using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NewsBus.DownloaderService.Core;

namespace NewsBus.DownloaderService.Tests
{
    [TestClass]
    public class ParserTests
    {
        [TestMethod]
        public async Task Process_RegularContent_ReturnCorrectContent()
        {
            ContentParser target = new ContentParser();
            string actual = await target.ProcessAsync(@"<html>bla-bla<article>content</article>bla-bla</html>");
            string expected = @"<article>content</article>";
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public async Task Process_ContentWithRandomCase_ReturnCorrectContent()
        {
            ContentParser target = new ContentParser();
            string actual = await target.ProcessAsync(@"<html>bla-bla<aRtiCle>content</ArticlE>bla-bla</html>");
            actual = actual.ToLower();
            string expected = @"<article>content</article>";
            Assert.AreEqual(expected, actual);
        }
    }
}