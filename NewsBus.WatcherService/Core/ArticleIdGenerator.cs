using System;
using System.Security.Cryptography;
using System.Text;
using NewsBus.Contracts;

namespace NewsBus.WatcherService.Core
{
    /// <summary>
    /// Generates an unique guid based on a given article url by a hash algorithm
    /// </summary>
    public class ArticleIdGenerator : IArticleIdGenerator, IDisposable
    {
        private readonly MD5CryptoServiceProvider md5;
        private bool isDisposed = false; // prevent redundant dispose

        public ArticleIdGenerator()
        {
            md5 = new MD5CryptoServiceProvider();
        }

        /// <summary>
        /// Convert the url to a guid by a hash algorithm
        /// </summary>
        /// <param name="url">article url</param>
        /// <returns>unique guid value</returns>
        public Guid Convert(string url)
        {
            byte[] hashBytes = md5.ComputeHash(Encoding.UTF8.GetBytes(url));
            return new Guid(hashBytes);
        }

        /// <summary>
        /// Dispose pattern
        /// </summary>
        /// <param name="disposing">if true it indicates that dispose is called explicitly</param>
        protected void Dispose(bool disposing)
        {
            if (disposing)
            {
                md5.Dispose();
            }
        }

        /// <summary>
        /// Dispose resources
        /// </summary>
        public void Dispose()
        {
            if (isDisposed)
            {
                return;
            }

            Dispose(true);
            GC.SuppressFinalize(this);
            isDisposed = true;
        }
    }
}