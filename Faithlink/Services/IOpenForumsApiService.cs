using Faithlink.Models;

namespace Faithlink.Services
{
    public interface IOpenForumsApiService
    {
        Task<IEnumerable<OpenForum>> GetOpenForumsAsync();
        Task JoinForumAsync(int forumId);
    }

}
