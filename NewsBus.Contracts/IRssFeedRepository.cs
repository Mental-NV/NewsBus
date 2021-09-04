using System.Collections.Generic;
using System.Threading.Tasks;
using NewsBus.Contracts.Models;

namespace NewsBus.Contracts
{
    public interface IRssFeedRepository
    {
        Task<IEnumerable<RssFeed>> GetItemsAsync();
    }
}