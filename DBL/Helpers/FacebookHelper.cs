using DBL.Models;
using Newtonsoft.Json.Linq;
using System.Text.Json;

namespace DBL.Helpers
{
    public class FacebookHelper
    {
        /// <summary>
        /// Exchanges a short-lived access token for a long-lived one (expires in two months).
        /// </summary>
        /// <param name="appId">The Facebook App ID.</param>
        /// <param name="appSecret">The Facebook App Secret.</param>
        /// <param name="shortLivedAccessToken">The short-lived access token.</param>
        /// <returns>The exchange token response.</returns>
        public async Task<FacebookExchangeTokenResponse> ExchangeAccessTokenAsync(string appId, string appSecret, string shortLivedAccessToken)
        {
            string requestUri = $"https://graph.facebook.com/oauth/access_token?client_id={appId}&client_secret={appSecret}&grant_type=fb_exchange_token&fb_exchange_token={shortLivedAccessToken}";

            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.GetAsync(requestUri);
                response.EnsureSuccessStatusCode();
                var responseBody = await response.Content.ReadAsStringAsync();

                return JsonSerializer.Deserialize<FacebookExchangeTokenResponse>(responseBody);
            }
        }

        /// <summary>
        /// Generates a never-expiring access token.
        /// </summary>
        /// <param name="extendedAccessToken">The extended access token.</param>
        /// <returns>The never expires token response.</returns>
        public async Task<FacebookNeverExpiresResponse> GenerateNeverExpiresAccessTokenAsync(string extendedAccessToken)
        {
            string requestUri = $"https://graph.facebook.com/me/accounts?access_token={extendedAccessToken}";

            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.GetAsync(requestUri);
                response.EnsureSuccessStatusCode();
                var responseBody = await response.Content.ReadAsStringAsync();

                return JsonSerializer.Deserialize<FacebookNeverExpiresResponse>(responseBody);
            }
        }


        /// <summary>
        /// Publish a blog post with a summary, images, and a link.
        /// </summary>
        /// <param name="summary">Summary of the blog post</param>
        /// <param name="imageUrls">List of image URLs to include</param>
        /// <param name="blogLink">Link to the blog post</param>
        /// <returns>Status message</returns>
        public async Task<string> PublishBlogPostAsync(string pageAccessToken, string postToPageURL, string summary, List<string> imageUrls, string blogLink)
        {
            try
            {
                var imageIds = new List<string>();
                foreach (var imageUrl in imageUrls)
                {
                    var uploadResult = await UploadPhotoAsync(pageAccessToken, postToPageURL, imageUrl);
                    if (uploadResult.Item1 != 200)
                    {
                        var error = ParseError(uploadResult.Item2);
                        return $"Error uploading photo to Facebook: {error}";
                    }

                    var uploadResponse = JObject.Parse(uploadResult.Item2);
                    imageIds.Add(uploadResponse["id"].Value<string>());
                }

                var pagePostResult = await PublishPostAsync(pageAccessToken, postToPageURL, summary, imageIds, blogLink);
                if (pagePostResult.Item1 != 200)
                {
                    var error = ParseError(pagePostResult.Item2);
                    return $"Error posting to Facebook page: {error}";
                }

                var profilePostResult = await PublishPostAsync(pageAccessToken, postToPageURL, summary, imageIds, blogLink);
                if (profilePostResult.Item1 != 200)
                {
                    var error = ParseError(profilePostResult.Item2);
                    return $"Error posting to Facebook profile: {error}";
                }

                return "Post published successfully!";
            }
            catch (Exception ex)
            {
                // Log exception somewhere
                return $"Unknown error publishing post to Facebook: {ex.Message}";
            }
        }

        private async Task<Tuple<int, string>> UploadPhotoAsync(string pageAccessToken, string postToPagePhotosURL, string photoUrl)
        {
            using (var http = new HttpClient())
            {
                var postData = new Dictionary<string, string>
            {
                { "access_token", pageAccessToken },
                { "url", photoUrl }
            };

                var httpResponse = await http.PostAsync(postToPagePhotosURL, new FormUrlEncodedContent(postData));
                var httpContent = await httpResponse.Content.ReadAsStringAsync();

                return new Tuple<int, string>((int)httpResponse.StatusCode, httpContent);
            }
        }

        private async Task<Tuple<int, string>> PublishPostAsync(string accessToken, string postUrl, string message, List<string> imageIds, string link)
        {
            using (var http = new HttpClient())
            {
                var postData = new Dictionary<string, string>
            {
                { "access_token", accessToken },
                { "message", message },
                { "link", link },
                { "attached_media", string.Join(",", imageIds.ConvertAll(id => $"{{\"media_fbid\":\"{id}\"}}")) }
            };

                var httpResponse = await http.PostAsync(postUrl, new FormUrlEncodedContent(postData));
                var httpContent = await httpResponse.Content.ReadAsStringAsync();

                return new Tuple<int, string>((int)httpResponse.StatusCode, httpContent);
            }
        }

        private string ParseError(string jsonResponse)
        {
            try
            {
                var errorJson = JObject.Parse(jsonResponse);
                return errorJson["error"]["message"].Value<string>();
            }
            catch
            {
                return "Unknown error";
            }
        }
    }
}
