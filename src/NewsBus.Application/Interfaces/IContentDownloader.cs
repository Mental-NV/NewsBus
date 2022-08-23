using System;
using System.Threading.Tasks;

namespace NewsBus.Application.Interfaces
{
    public interface IContentDownloader
    {
        Task<string> GetAsync(Uri url);
    }
}