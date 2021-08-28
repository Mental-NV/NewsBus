using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NewsBus.Domain.Models;

namespace NewsBus.Domain
{
    public interface IRssLoader
    {
        Task<IEnumerable<MetaArticle>> LoadAsync(Uri rssFeedUrl);
    }
}