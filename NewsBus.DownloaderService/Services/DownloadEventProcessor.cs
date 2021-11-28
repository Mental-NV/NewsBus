using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NewsBus.Application.Interfaces;
using NewsBus.Domain.Models;

namespace NewsBus.DownloaderService
{
    public class DownloadEventProcessor : IDownloadEventProcessor
    {
        private readonly IArticleRepository articleRepository;
        private readonly IArticleContentRepository articleContentRepository;
        private readonly IContentDownloader downloader;
        private readonly IContentParser parser;
        private readonly ILogger<DownloadEventProcessor> logger;

        public DownloadEventProcessor(IArticleRepository articleRepository,
                                      IArticleContentRepository articleContentRepository,
                                      IContentDownloader downloader,
                                      IContentParser parser,
                                      ILogger<DownloadEventProcessor> logger)
        {
            this.articleRepository = articleRepository ?? throw new System.ArgumentNullException(nameof(articleRepository));
            this.articleContentRepository = articleContentRepository ?? throw new System.ArgumentNullException(nameof(articleContentRepository));
            this.downloader = downloader ?? throw new System.ArgumentNullException(nameof(downloader));
            this.parser = parser ?? throw new System.ArgumentNullException(nameof(parser));
            this.logger = logger ?? throw new System.ArgumentNullException(nameof(logger));
        }

        public async Task Process(Article article)
        {
            string articleContent = await downloader.GetAsync(article.Url);
            articleContent = await parser.ProcessAsync(articleContent);
            
            if (await articleRepository.Exist(article.Id))
            {
                logger.LogWarning($"Skipped duplicated article {article.Id}");
                return;
            }

            bool success = await articleContentRepository.PostContentAsync(article.Id, articleContent);
            if (success)
            {
                await articleRepository.PostArticleAsync(article);
                logger.LogInformation($"Added article {article.Id}");
            }
        }
    }
}