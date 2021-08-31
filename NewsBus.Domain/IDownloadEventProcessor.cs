using System.Threading.Tasks;
using NewsBus.Domain.Models;

namespace NewsBus.Domain
{
    public interface IDownloadEventProcessor
    {
        Task Process(Article article);
    }
}