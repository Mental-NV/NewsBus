using System.Collections.Generic;
using System.Threading.Tasks;
using NewsBus.Domain.Models;

namespace NewsBus.Application.Interfaces
{
    /// <summary>
    /// Represents data access to the article repository
    /// </summary>
    public interface IArticleRepository
    {
        /// <summary>
        /// Read all articles from the repository
        /// </summary>
        /// <returns>list of articles</returns>
        Task<IEnumerable<Article>> GetArticlesAsync();

        /// <summary>
        /// Read an article from the repository (slow version)
        /// </summary>
        /// <param name="id">the article id</param>
        /// <returns>the article data model</returns>
        Task<Article> GetArticleAsync(string id);

        /// <summary>
        /// Read an article from the repository
        /// </summary>
        /// <param name="id">the article id</param>
        /// <param name="url">the article url (partition key)</param>
        /// <returns>the article data model</returns>
        Task<Article> GetArticleAsync(string id, string url);

        /// <summary>
        /// Check if the article is already exist in the repository
        /// </summary>
        /// <param name="id">the article id</param>
        /// <returns>true if the article exists</returns>
        Task<bool> Exist(string id);

        /// <summary>
        /// Create a new article in the repository
        /// </summary>
        /// <param name="article">the article</param>
        /// <returns>true if success</returns>
        Task<bool> PostArticleAsync(Article article);

        /// <summary>
        /// Update an article in the repository
        /// </summary>
        /// <param name="id">the article id</param>
        /// <param name="article">the article</param>
        /// <returns>true if success</returns>
        Task<bool> PutArticleAsync(string id, Article article);
        
        /// <summary>
        /// Delete an article from the repository
        /// </summary>
        /// <param name="id">the article id</param>
        /// <param name="url">the article url (partition key)</param>
        /// <returns>true if success</returns>
        Task<bool> DeleteArticleAsync(string id, string url);
    }
}