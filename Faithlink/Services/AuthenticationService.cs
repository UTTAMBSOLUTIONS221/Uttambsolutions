using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Faithlink.Services
{
    public class AuthenticationService
    {
        private readonly HttpClient _httpClient;

        public AuthenticationService()
        {
            _httpClient = new HttpClient();
            // Replace with your API base URL
            _httpClient.BaseAddress = new Uri("https://your-api-base-url.com/");
        }

        public async Task<bool> LoginAsync(string username, string password)
        {
            var loginModel = new
            {
                Username = username,
                Password = password
            };

            var content = new StringContent(JsonConvert.SerializeObject(loginModel), Encoding.UTF8, "application/json");

            try
            {
                var response = await _httpClient.PostAsync("api/auth/login", content);

                if (response.IsSuccessStatusCode)
                {
                    // Process successful login (e.g., store token)
                    var token = await response.Content.ReadAsStringAsync();
                    // Store token securely (e.g., in secure storage or token cache)
                    // Implement your token storage logic here
                    return true;
                }
                else
                {
                    // Handle unsuccessful login
                    return false;
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions
                Console.WriteLine($"Exception: {ex.Message}");
                return false;
            }
        }
    }
}
