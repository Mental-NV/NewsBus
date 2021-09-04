using System.Threading.Tasks;

namespace NewsBus.Contracts
{
    public interface IContentParser
    {
        Task<string> ProcessAsync(string rawContent);
    }
}