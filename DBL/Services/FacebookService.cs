using DBL.Entities;
using DBL.Models;
using System.Text.Json;

namespace DBL.Services
{
    public class FacebookService
    {
        private readonly HttpClient _httpClient;

        public FacebookService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<FacebookAccessTokenResponse> GetAccessTokenAsync(string appId, string appSecret)
        {
            var requestUri = $"https://graph.facebook.com/oauth/access_token?client_id={appId}&client_secret={appSecret}&grant_type=client_credentials";

            var response = await _httpClient.GetAsync(requestUri);
            response.EnsureSuccessStatusCode();
            var responseBody = await response.Content.ReadAsStringAsync();

            var accessTokenResponse = JsonSerializer.Deserialize<FacebookAccessTokenResponse>(responseBody);

            return accessTokenResponse;
        }

        //get access token long lived..,,expires in two months
        public async Task<FacebookExchangeTokenResponse> ExchangeAccessTokenAsync(string appId, string appSecret, string shortLivedAccessToken)
        {
            string requestUri = $"https://graph.facebook.com/oauth/access_token?client_id={appId}&client_secret={appSecret}&grant_type=fb_exchange_token&fb_exchange_token={shortLivedAccessToken}";

            var response = await _httpClient.GetAsync(requestUri);
            response.EnsureSuccessStatusCode();
            var responseBody = await response.Content.ReadAsStringAsync();

            var exchangeTokenResponse = JsonSerializer.Deserialize<FacebookExchangeTokenResponse>(responseBody);

            return exchangeTokenResponse;
        }
        //get access token never expires
        public async Task<FacebookNeverExpiresResponse> Generatenevereexpiresaccesstoken(string extendedaccesstoken)
        {
            string requestUri = $"https://graph.facebook.com/me/accounts?access_token={extendedaccesstoken}";

            var response = await _httpClient.GetAsync(requestUri);
            response.EnsureSuccessStatusCode();
            var responseBody = await response.Content.ReadAsStringAsync();

            var neverExpiresTokenResponse = JsonSerializer.Deserialize<FacebookNeverExpiresResponse>(responseBody);

            return neverExpiresTokenResponse;
        }

        public async Task<string> PostBlogToFacebook(string accessToken, Systemblog blogPost)
        {
            var url = "https://graph.facebook.com/me/feed";

            var parameters = new Dictionary<string, string>
            {
                { "message", $"{blogPost.Blogname}\n{blogPost.Summary}" },
                { "link", "blogPost.BlogUrl" },
                { "picture", blogPost.Blogprimaryimageurl },
                { "access_token", accessToken }
            };

            var content = new FormUrlEncodedContent(parameters);
            var response = await _httpClient.PostAsync(url, content);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsStringAsync();
            }
            else
            {
                string errorResponse = await response.Content.ReadAsStringAsync();
                throw new Exception($"Error posting to Facebook: {response.StatusCode} - {errorResponse}");
            }
        }
    }
}
