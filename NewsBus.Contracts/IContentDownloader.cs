using System;
using System.Threading.Tasks;

namespace NewsBus.Contracts
{
    public interface IContentDownloader
    {
        Task<string> GetAsync(Uri url);
    }
}