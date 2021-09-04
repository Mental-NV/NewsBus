using System;
using System.Threading.Tasks;
using NewsBus.Contracts;

namespace NewsBus.DownloaderService.Core
{
    public class ContentParser : IContentParser
    {
        public Task<string> ProcessAsync(string rawContent)
        {
            if (string.IsNullOrWhiteSpace(rawContent))
            {
                return Task.FromResult(rawContent);
            }

            int startPos = rawContent.IndexOf("<article", StringComparison.OrdinalIgnoreCase);
            int endPos = rawContent.IndexOf("</article>", StringComparison.OrdinalIgnoreCase);
            string content = rawContent.Substring(startPos, endPos - startPos + "</article>".Length);
            return Task.FromResult(content);
        }
    }
}