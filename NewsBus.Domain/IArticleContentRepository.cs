using System.Threading.Tasks;

namespace NewsBus.Domain
{
    public interface IArticleContentRepository
    {
        Task<string> GetContent(string id);
        Task<bool> PostContentAsync(string id, string content);
    }
}