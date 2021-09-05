using System;

namespace NewsBus.Application.Interfaces
{
    /// <summary>
    /// Generates an unique guid based on a given article url by a hash algorithm
    /// </summary>
    public interface IArticleIdGenerator
    {
        /// <summary>
        /// Convert the url to a guid by a hash algorithm
        /// </summary>
        /// <param name="url">article url</param>
        /// <returns>unique guid value</returns>
        Guid Convert(string url);
    }
}