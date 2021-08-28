using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NewsBus.Domain;
using NewsBus.Domain.Models;

namespace NewsBus.WatcherService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ArticleController : Controller
    {
        private readonly IRssFeedRepository rssFeedRepository;
        private readonly IRssLoader rssLoader;
        public ArticleController(IRssFeedRepository rssFeedRepository, IRssLoader rssLoader)
        {
            this.rssLoader = rssLoader ?? throw new System.ArgumentNullException(nameof(rssLoader));
            this.rssFeedRepository = rssFeedRepository ?? throw new System.ArgumentNullException(nameof(rssFeedRepository));
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            IEnumerable<RssFeed> rssFeeds = await rssFeedRepository.GetItemsAsync();
            RssFeed rssFeed = rssFeeds.FirstOrDefault();
            IEnumerable<MetaArticle> articles = await rssLoader.LoadAsync(rssFeed.Url);
            return Json(articles);
        }
    }
}