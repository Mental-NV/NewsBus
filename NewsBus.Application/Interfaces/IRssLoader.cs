using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NewsBus.Domain.Models;

namespace NewsBus.Application.Interfaces
{
    public interface IRssLoader
    {
        Task<IEnumerable<Article>> LoadAsync(Uri rssFeedUrl);
    }
}