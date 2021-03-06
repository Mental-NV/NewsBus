using System.Threading.Tasks;

namespace NewsBus.Application.Interfaces
{
    public interface IArticleContentRepository
    {
        Task<string> GetContentAsync(string id);
        Task<bool> PostContentAsync(string id, string content);
        Task<bool> DeleteContentAsync(string id);
    }
}