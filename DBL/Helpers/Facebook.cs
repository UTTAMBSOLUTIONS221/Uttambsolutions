using Newtonsoft.Json.Linq;

namespace DBL.Helpers
{
    public class Facebook
    {
        private readonly string _accessToken;
        private readonly string _pageAccessToken;
        private readonly string _pageID;
        private readonly string _facebookAPI = "https://graph.facebook.com/";
        private readonly string _postToPageURL;
        private readonly string _postToProfileURL;
        private readonly string _postToPagePhotosURL;

        public Facebook(string accessToken, string pageAccessToken, string pageID)
        {
            _accessToken = accessToken;
            _pageAccessToken = pageAccessToken;
            _pageID = pageID;
            _postToPageURL = $"{_facebookAPI}{pageID}/feed";
            _postToProfileURL = $"{_facebookAPI}me/feed";
            _postToPagePhotosURL = $"{_facebookAPI}{pageID}/photos";
        }

        /// <summary>
        /// Publish a blog post with a summary, images, and a link.
        /// </summary>
        /// <param name="summary">Summary of the blog post</param>
        /// <param name="imageUrls">List of image URLs to include</param>
        /// <param name="blogLink">Link to the blog post</param>
        /// <returns>Status message</returns>
        //public async Task<string> PublishBlogPostAsync(string summary, List<string> imageUrls, string blogLink)
        //{
        //    try
        //    {
        //        var imageIds = new List<string>();
        //        foreach (var imageUrl in imageUrls)
        //        {
        //            var uploadResult = await UploadPhotoAsync(imageUrl);
        //            if (uploadResult.Item1 != 200)
        //            {
        //                var error = ParseError(uploadResult.Item2);
        //                return $"Error uploading photo to Facebook: {error}";
        //            }

        //            var uploadResponse = JObject.Parse(uploadResult.Item2);
        //            imageIds.Add(uploadResponse["id"].Value<string>());
        //        }

        //        var pagePostResult = await PublishPostAsync(_pageAccessToken, _postToPageURL, summary, imageIds, blogLink);
        //        if (pagePostResult.Item1 != 200)
        //        {
        //            var error = ParseError(pagePostResult.Item2);
        //            return $"Error posting to Facebook page: {error}";
        //        }

        //        var profilePostResult = await PublishPostAsync(_accessToken, _postToProfileURL, summary, imageIds, blogLink);
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

        //private async Task<Tuple<int, string>> UploadPhotoAsync(string photoUrl)
        //{
        //    using (var http = new HttpClient("", "", null))
        //    {
        //        var postData = new Dictionary<string, string>
        //    {
        //        { "access_token", _pageAccessToken },
        //        { "url", photoUrl }
        //    };

        //        var httpResponse = await http.PostAsync(_postToPagePhotosURL, new FormUrlEncodedContent(postData));
        //        var httpContent = await httpResponse.Content.ReadAsStringAsync();

        //        return new Tuple<int, string>((int)httpResponse.StatusCode, httpContent);
        //    }
        //}

        //private async Task<Tuple<int, string>> PublishPostAsync(string accessToken, string postUrl, string message, List<string> imageIds, string link)
        //{
        //    using (var http = new HttpClient())
        //    {
        //        var postData = new Dictionary<string, string>
        //    {
        //        { "access_token", accessToken },
        //        { "message", message },
        //        { "link", link },
        //        { "attached_media", string.Join(",", imageIds.ConvertAll(id => $"{{\"media_fbid\":\"{id}\"}}")) }
        //    };

        //        var httpResponse = await http.PostAsync(postUrl, new FormUrlEncodedContent(postData));
        //        var httpContent = await httpResponse.Content.ReadAsStringAsync();

        //        return new Tuple<int, string>((int)httpResponse.StatusCode, httpContent);
        //    }
        //}

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
