using System.Threading.Tasks;
using NewsBus.Contracts.Models;

namespace NewsBus.Contracts
{
    public interface IDownloadEventProcessor
    {
        Task Process(Article article);
    }
}