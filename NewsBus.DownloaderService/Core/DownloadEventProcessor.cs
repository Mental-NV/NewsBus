using System.Threading.Tasks;
using NewsBus.Contracts;
using NewsBus.Contracts.Models;

namespace NewsBus.DownloaderService.Core
{
    public class DownloadEventProcessor : IDownloadEventProcessor
    {
        private readonly IArticleRepository articleRepository;
        private readonly IArticleContentRepository articleContentRepository;
        private readonly IContentDownloader downloader;
        private readonly IContentParser parser;

        public DownloadEventProcessor(IArticleRepository articleRepository,
                                      IArticleContentRepository articleContentRepository,
                                      IContentDownloader downloader,
                                      IContentParser parser)
        {
            this.articleRepository = articleRepository ?? throw new System.ArgumentNullException(nameof(articleRepository));
            this.articleContentRepository = articleContentRepository ?? throw new System.ArgumentNullException(nameof(articleContentRepository));
            this.downloader = downloader ?? throw new System.ArgumentNullException(nameof(downloader));
            this.parser = parser ?? throw new System.ArgumentNullException(nameof(parser));
        }

        public async Task Process(Article article)
        {
            string articleContent = await downloader.GetAsync(article.Url);
            articleContent = await parser.ProcessAsync(articleContent);
            bool success = await articleContentRepository.PostContentAsync(article.Id, articleContent);
            if (success)
            {
                await articleRepository.PostArticleAsync(article);
            }
        }
    }
}