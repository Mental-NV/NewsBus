using System.Threading.Tasks;
using NewsBus.Domain.Models;

namespace NewsBus.Application.Interfaces
{
    public interface IDownloadEventProcessor
    {
        Task Process(Article article);
    }
}