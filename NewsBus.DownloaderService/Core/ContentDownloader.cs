using System;
using System.Net.Http;
using System.Threading.Tasks;
using NewsBus.Contracts;

namespace NewsBus.DownloaderService.Core
{
    public class ContentDownloader : IContentDownloader, IDisposable
    {
        private readonly HttpClient client;
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

        public void Dispose()
        {
            client?.Dispose();
        }
    }
}