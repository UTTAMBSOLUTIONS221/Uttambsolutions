using Faithlink.Models;
using Newtonsoft.Json;
using System.Text;

namespace Faithlink.Services
{
    public class GroupManagementService : IGroupManagementService
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUri = "https://api.faithlink.com"; // Replace with your actual API base URL

        public GroupManagementService()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri(_baseUri);
        }

        public async Task<List<GroupModel>> LoadGroupsAsync()
        {
            var response = await _httpClient.GetAsync("/groups");
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var groups = JsonConvert.DeserializeObject<List<GroupModel>>(content);

            return groups;
        }

        public async Task JoinGroupAsync(GroupModel group, long userId)
        {
            var requestBody = new
            {
                GroupId = group.Id,
                UserId = userId
            };

            var json = JsonConvert.SerializeObject(requestBody);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("/groups/join", httpContent);
            response.EnsureSuccessStatusCode();

            // Optionally handle response content if needed
        }

        // Add other methods for group management (create, update, delete, etc.) as needed
    }
}
