using System;
using System.Net.Http;
using System.Threading.Tasks;
using NewsBus.Application.Interfaces;

namespace NewsBus.Infrastructure
{
    public class ContentDownloader : IContentDownloader, IDisposable
    {
        private readonly HttpClient client;
        private bool isDisposed = false;
        public ContentDownloader()
        {
            client = new HttpClient();
        }

        public async Task<string> GetAsync(Uri url)
        {
            HttpResponseMessage response = await client.GetAsync(url);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        protected void Dispose(bool disposing)
        {
            if (disposing)
            {
                client.Dispose();
            }
        }

        public void Dispose()
        {
            if (!isDisposed)
            {
                Dispose(true);
            }
            isDisposed = true;
        }
    }
}