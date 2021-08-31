using System.Collections.Generic;
using System.Threading.Tasks;
using NewsBus.Domain.Models;

namespace NewsBus.Domain
{
    public interface IArticleRepository
    {
        Task<IEnumerable<Article>> GetArticlesAsync();

        Task<Article> GetArticleAsync(string id);

        Task<Article> GetArticleAsync(string id, string url);

        Task<bool> PostArticleAsync(Article article);

        Task<bool> PutArticleAsync(string id, Article article);

        Task<bool> DeleteArticleAsync(string id, string url);
    }
}