using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NewsBus.Contracts.Models;

namespace NewsBus.Contracts
{
    public interface IRssLoader
    {
        Task<IEnumerable<Article>> LoadAsync(Uri rssFeedUrl);
    }
}