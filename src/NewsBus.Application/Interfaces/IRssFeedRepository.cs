using System.Collections.Generic;
using System.Threading.Tasks;
using NewsBus.Domain.Models;

namespace NewsBus.Application.Interfaces
{
    public interface IRssFeedRepository
    {
        Task<IEnumerable<RssFeed>> GetItemsAsync();
    }
}