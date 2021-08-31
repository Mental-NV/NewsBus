using System;
using System.Threading.Tasks;

namespace NewsBus.Domain
{
    public interface IContentDownloader
    {
        Task<string> GetAsync(Uri url);
    }
}