using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NewsBus.Contracts;
using NewsBus.Contracts.Models;

namespace NewsBus.WatcherService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RssFeedController : Controller
    {
        private readonly IRssFeedRepository rssFeedRepository;
        public RssFeedController(IRssFeedRepository rssFeedRepository)
        {
            this.rssFeedRepository = rssFeedRepository ?? throw new System.ArgumentNullException(nameof(rssFeedRepository));
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            IEnumerable<RssFeed> feeds = await rssFeedRepository.GetItemsAsync();
            return this.Json(feeds);
        }
    }
}