
using Faithlink.Models;
using Newtonsoft.Json;

namespace Faithlink.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly HttpClient _httpClient;

        public AuthenticationService()
        {
            _httpClient = new HttpClient();
            // Configure HttpClient, base address, headers, etc.
        }
        public async Task<UsermodelResponce> Validateuser(string email, string password)
        {
            // Example URL and request body for login endpoint
            string apiUrl = "http://mainapi.uttambsolutions.com/api/Account/Authenticate";
            var requestBody = new { Email = email, Password = password };

            try
            {
                HttpResponseMessage response = await _httpClient.PostAsync(apiUrl, new StringContent(JsonConvert.SerializeObject(requestBody)));
                response.EnsureSuccessStatusCode(); // Ensure success status code

                string jsonResponse = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<UsermodelResponce>(jsonResponse);
            }
            catch (Exception ex)
            {
                // Handle exception (e.g., logging, error handling)
                throw new ApplicationException("Error occurred while logging in", ex);
            }
        }
    }
}
