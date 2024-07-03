using Faithlink.Models;
using System.Net.Http.Json;
namespace Faithlink.Services
{
    public class OpenForumsApiService : IOpenForumsApiService
    {
        private readonly HttpClient _httpClient;

        public OpenForumsApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<OpenForum>> GetOpenForumsAsync()
        {
            var response = await _httpClient.GetAsync("api/openforums");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<IEnumerable<OpenForum>>();
        }

        public async Task JoinForumAsync(int forumId)
        {
            var response = await _httpClient.PostAsync($"api/openforums/{forumId}/join", null);
            response.EnsureSuccessStatusCode();
        }
    }

}
