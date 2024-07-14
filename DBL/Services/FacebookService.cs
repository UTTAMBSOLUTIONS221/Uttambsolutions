using DBL.Entities;

namespace DBL.Services
{
    public class FacebookService
    {
        private readonly HttpClient _httpClient;

        public FacebookService()
        {
            _httpClient = new HttpClient();
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
