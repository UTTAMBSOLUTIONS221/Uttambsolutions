namespace DBL.Helpers
{
    //public class LinkedinHelpers
    //{
    //    private readonly HttpClient _client;

    //    public LinkedinHelpers()
    //    {
    //        _client = new HttpClient();
    //    }
    //    public async Task<string> GetAuthorizationUrl(string clientId, string redirectUri, string state)
    //    {
    //        return $"https://www.linkedin.com/oauth/v2/authorization?response_type=code&client_id={clientId}&redirect_uri={Uri.EscapeDataString(redirectUri)}&state={state}&scope=w_member_social";
    //    }
    //    public async Task<string> GetAccessTokenAsync(string clientId, string clientSecret, string redirectUri, string authCode)
    //    {
    //        var requestBody = new Dictionary<string, string>
    //        {
    //            { "grant_type", "authorization_code" },
    //            { "code", authCode },
    //            { "redirect_uri", redirectUri },
    //            { "client_id", clientId },
    //            { "client_secret", clientSecret }
    //        };

    //        var requestContent = new FormUrlEncodedContent(requestBody);
    //        var response = await _client.PostAsync("https://www.linkedin.com/oauth/v2/accessToken", requestContent);

    //        if (!response.IsSuccessStatusCode)
    //        {
    //            // Log error or throw exception
    //            var errorContent = await response.Content.ReadAsStringAsync();
    //            throw new HttpRequestException($"Error fetching access token: {errorContent}");
    //        }

    //        var responseContent = await response.Content.ReadAsStringAsync();
    //        var tokenResponse = JsonConvert.DeserializeObject<TokenResponse>(responseContent);

    //        return tokenResponse.AccessToken;
    //    }

    //    public async Task PostJobToLinkedInAsync(string accessToken, SystemJob jobPost, string companyPageId)
    //    {
    //        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

    //        var postContent = new
    //        {
    //            author = $"urn:li:organization:{companyPageId}",
    //            lifecycleState = "PUBLISHED",
    //            specificContent = new
    //            {
    //                comLinkedinUgcShareContent = new
    //                {
    //                    shareCommentary = new
    //                    {
    //                        text = $"{jobPost.Title}\n\n{jobPost.JobDescription}\n\nApply here: {jobPost.JobPostUrl}"
    //                    },
    //                    shareMediaCategory = "ARTICLE"
    //                }
    //            },
    //            visibility = new
    //            {
    //                comLinkedinUgcMemberNetworkVisibility = "PUBLIC"
    //            }
    //        };

    //        var jsonContent = JsonConvert.SerializeObject(postContent);
    //        var requestContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

    //        var response = await _client.PostAsync("https://api.linkedin.com/v2/ugcPosts", requestContent);

    //        if (!response.IsSuccessStatusCode)
    //        {
    //            // Log error or throw exception
    //            var errorContent = await response.Content.ReadAsStringAsync();
    //            throw new HttpRequestException($"Error posting job to LinkedIn: {errorContent}");
    //        }
    //    }

    //    public class TokenResponse
    //    {
    //        [JsonProperty("access_token")]
    //        public string AccessToken { get; set; }
    //    }

    //    public class JobPost
    //    {
    //        public string Title { get; set; }
    //        public string Description { get; set; }
    //        public string Url { get; set; }
    //    }
    //}
}
