namespace DBL.Helpers
{
    public class FacebookHelper
    {
        //public async Task<FacebookExchangeTokenResponse> ExchangeAccessTokenAsync(string appId, string appSecret, string shortLivedAccessToken)
        //{
        //    string requestUri = $"https://graph.facebook.com/oauth/access_token?client_id={appId}&client_secret={appSecret}&grant_type=fb_exchange_token&fb_exchange_token={shortLivedAccessToken}";

        //    using (var httpClient = new HttpClient())
        //    {
        //        var response = await httpClient.GetAsync(requestUri);
        //        response.EnsureSuccessStatusCode();
        //        var responseBody = await response.Content.ReadAsStringAsync();
        //        return JsonConvert.DeserializeObject<FacebookExchangeTokenResponse>(responseBody);
        //    }
        //}

        //public async Task<FacebookNeverExpiresResponse> GenerateNeverExpiresAccessTokenAsync(string extendedAccessToken)
        //{
        //    string requestUri = $"https://graph.facebook.com/me/accounts?access_token={extendedAccessToken}";

        //    using (var httpClient = new HttpClient())
        //    {
        //        var response = await httpClient.GetAsync(requestUri);
        //        response.EnsureSuccessStatusCode();
        //        var responseBody = await response.Content.ReadAsStringAsync();

        //        return JsonConvert.DeserializeObject<FacebookNeverExpiresResponse>(responseBody);
        //    }
        //}

        ///// <summary>
        ///// Publish a blog post with a summary, images, and a link.
        ///// </summary>
        ///// <param name="summary">Summary of the blog post</param>
        ///// <param name="imageUrls">List of image URLs to include</param>
        ///// <param name="blogLink">Link to the blog post</param>
        ///// <returns>Status message</returns>
        //public async Task<string> PublishBlogPostAsync(string pageAccessToken, string userAccessToken, string pageID, string summary, string blogLink)
        //{
        //    try
        //    {
        //        string postToPageURL = $"https://graph.facebook.com/{pageID}/feed";
        //        string postToProfileURL = $"https://graph.facebook.com/me/feed";

        //        // Publish post to the Facebook page
        //        var pagePostResult = await PublishPostAsync(pageAccessToken, postToPageURL, summary, blogLink);
        //        if (pagePostResult.Item1 != 200)
        //        {
        //            var error = ParseError(pagePostResult.Item2);
        //            return $"Error posting to Facebook page: {error}";
        //        }

        //        // Publish post to the Facebook profile
        //        var profilePostResult = await PublishPostAsync(userAccessToken, postToProfileURL, summary, blogLink);
        //        if (profilePostResult.Item1 != 200)
        //        {
        //            var error = ParseError(profilePostResult.Item2);
        //            return $"Error posting to Facebook profile: {error}";
        //        }

        //        return "Post published successfully!";
        //    }
        //    catch (Exception ex)
        //    {
        //        // Log exception somewhere
        //        return $"Unknown error publishing post to Facebook: {ex.Message}";
        //    }
        //}

        //private async Task<Tuple<int, string>> PublishPostAsync(string accessToken, string postUrl, string message, string link)
        //{
        //    using (var http = new HttpClient())
        //    {
        //        var postData = new Dictionary<string, string>
        //{
        //    { "access_token", accessToken },
        //    { "message", message },
        //    { "link", link }
        //};

        //        var httpResponse = await http.PostAsync(postUrl, new FormUrlEncodedContent(postData));
        //        var httpContent = await httpResponse.Content.ReadAsStringAsync();

        //        return new Tuple<int, string>((int)httpResponse.StatusCode, httpContent);
        //    }
        //}

        //private string ParseError(string jsonResponse)
        //{
        //    try
        //    {
        //        var errorJson = JObject.Parse(jsonResponse);
        //        return errorJson["error"]["message"].Value<string>();
        //    }
        //    catch
        //    {
        //        return "Unknown error";
        //    }
        //}
    }
}
