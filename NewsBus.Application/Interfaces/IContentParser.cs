using System.Threading.Tasks;

namespace NewsBus.Application.Interfaces
{
    public interface IContentParser
    {
        Task<string> ProcessAsync(string rawContent);
    }
}