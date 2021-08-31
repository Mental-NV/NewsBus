using System.Threading.Tasks;

namespace NewsBus.Domain
{
    public interface IContentParser
    {
        Task<string> ProcessAsync(string rawContent);
    }
}