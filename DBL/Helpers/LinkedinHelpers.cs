using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace DBL.Helpers
{
    public class LinkedinHelpers
    {
        public async Task<string> GetAccessTokenAsync(string clientId, string clientSecret, string redirectUri, string authCode)
        {
            var client = new HttpClient();
            var requestBody = new Dictionary<string, string>
            {
                { "grant_type", "authorization_code" },
                { "code", authCode },
                { "redirect_uri", redirectUri },
                { "client_id", clientId },
                { "client_secret", clientSecret }
            };

            var requestContent = new FormUrlEncodedContent(requestBody);
            var response = await client.PostAsync("https://www.linkedin.com/oauth/v2/accessToken", requestContent);

            var responseContent = await response.Content.ReadAsStringAsync();
            var tokenResponse = JsonConvert.DeserializeObject<TokenResponse>(responseContent);

            return tokenResponse.AccessToken;
        }
        public async Task PostJobToLinkedInAsync(string accessToken, JobPost jobPost)
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            var postContent = new
            {
                author = "urn:li:organization:<YOUR_COMPANY_PAGE_ID>",
                lifecycleState = "PUBLISHED",
                specificContent = new
                {
                    "com.linkedin.ugc.ShareContent" = new
                    {
                        shareCommentary = new
                        {
                            text = $"{jobPost.Title}\n\n{jobPost.Description}\n\nApply here: {jobPost.Url}"
                        },
                        shareMediaCategory = "ARTICLE"
                    }
                },
                visibility = new
                {
                    "com.linkedin.ugc.MemberNetworkVisibility" = "PUBLIC"
                }
            };

            var jsonContent = JsonConvert.SerializeObject(postContent);
            var requestContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            var response = await client.PostAsync("https://api.linkedin.com/v2/ugcPosts", requestContent);

            if (!response.IsSuccessStatusCode)
            {
                // Handle error
            }
        }

        public class TokenResponse
        {
            [JsonProperty("access_token")]
            public string AccessToken { get; set; }
        }

    }
}
