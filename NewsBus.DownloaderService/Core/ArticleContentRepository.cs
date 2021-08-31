using System.IO;
using System.Text;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using NewsBus.Domain;

namespace NewsBus.DownloaderService.Core
{
    public class ArticleContentRepository : IArticleContentRepository
    {
        const string containerName = "articleblobs";
        private readonly BlobContainerClient container;
        public ArticleContentRepository(string blobStorageConnectionString)
        {
            if (string.IsNullOrWhiteSpace(blobStorageConnectionString))
            {
                throw new System.ArgumentException($"'{nameof(blobStorageConnectionString)}' cannot be null or whitespace.", nameof(blobStorageConnectionString));
            }

            container = new BlobContainerClient(blobStorageConnectionString, containerName);
            container.CreateIfNotExists();
        }

        public async Task<string> GetContentAsync(string id)
        {
            BlobClient blob = container.GetBlobClient(id);
            var response = await blob.DownloadContentAsync();
            return response.Value.Content.ToString();
        }

        public async Task<bool> PostContentAsync(string id, string content)
        {
            BlobClient blob = container.GetBlobClient(id);
            var response = await blob.UploadAsync(
                new MemoryStream(Encoding.UTF8.GetBytes(content)),
                new BlobHttpHeaders()
                {
                    ContentType = "application/json"
                });
            return true;
        }  
    }
}