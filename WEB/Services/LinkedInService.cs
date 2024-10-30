using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace WEB.Services
{
    public class LinkedInService
    {
        private readonly HttpClient _httpClient;

        public LinkedInService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string> PostJobToLinkedInAsync(string accessToken, string organizationId, string jobTitle, string jobUrl, string jobDescription, string imageUrn)
        {
            var postContent = new
            {
                author = $"urn:li:organization:{organizationId}",
                lifecycleState = "PUBLISHED",
                specificContent = new
                {
                    comLinkedinUgcShareContent = new
                    {
                        shareCommentary = new
                        {
                            text = "We're hiring! Check out this job opportunity."
                        },
                        shareMediaCategory = "IMAGE",
                        media = new[]
                        {
                        new
                        {
                            status = "READY",
                            description = new { text = jobDescription },
                            media = imageUrn,
                            title = new { text = jobTitle }
                        }
                    }
                    }
                },
                visibility = new
                {
                    comLinkedinUgcMemberNetworkVisibility = "PUBLIC"
                }
            };

            string jsonContent = JsonConvert.SerializeObject(postContent);

            using var requestMessage = new HttpRequestMessage(HttpMethod.Post, "https://api.linkedin.com/v2/ugcPosts")
            {
                Content = new StringContent(jsonContent, Encoding.UTF8, "application/json")
            };
            requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            HttpResponseMessage response = await _httpClient.SendAsync(requestMessage);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsStringAsync(); // Success
            }
            else
            {
                string error = await response.Content.ReadAsStringAsync();
                throw new Exception($"Error posting to LinkedIn: {error}");
            }
        }
    }
}
