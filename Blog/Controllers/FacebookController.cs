﻿using DBL;
using DBL.Entities;
using DBL.Helpers;
using DBL.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text.Json;

namespace Blog.Controllers
{
    [ApiController]
    [Route("api/facebook")]
    public class FacebookController : ControllerBase
    {
        private readonly FacebookHelper _facebookHelper;
        private readonly BL bl;
        IConfiguration _config;

        public FacebookController(FacebookHelper facebookHelper, IConfiguration config)
        {
            _facebookHelper = facebookHelper;
            bl = new BL(Util.ShareConnectionString(config));
            _config = config;
        }

        [HttpGet("login")]
        public IActionResult Login([FromQuery] string appId, [FromQuery] string appSecret, [FromQuery] string redirectUri)
        {
            var authorizationUrl = $"https://www.facebook.com/v11.0/dialog/oauth?client_id={appId}&redirect_uri={redirectUri}&scope=public_profile,email,pages_show_list,publish_pages,publish_to_groups";
            return Redirect(authorizationUrl);
        }

        [HttpGet("callback")]
        public async Task<IActionResult> Callback(
            [FromQuery] string code,
            [FromQuery] string appId,
            [FromQuery] string appSecret,
            [FromQuery] string pageName,
            [FromQuery] long socialOwner,  // You can pass the social owner ID if needed
            [FromQuery] long createdBy,     // You can pass the creator ID if needed
            [FromQuery] string pageType)   // Optional: Page type if applicable
        {
            if (string.IsNullOrEmpty(code))
            {
                return BadRequest("Authorization code is missing.");
            }

            if (string.IsNullOrEmpty(appId) || string.IsNullOrEmpty(appSecret))
            {
                return BadRequest("App ID and App Secret are required.");
            }

            // Step 1: Get the short-lived access token
            var shortLivedToken = await GetShortLivedAccessTokenAsync(code, appId, appSecret);

            // Step 2: Exchange the short-lived token for a long-lived access token (user access token)
            var longLivedToken = await _facebookHelper.ExchangeAccessTokenAsync(appId, appSecret, shortLivedToken);

            // Step 3: Generate page access tokens (if applicable)
            var pageAccessTokenResponse = await _facebookHelper.GenerateNeverExpiresAccessTokenAsync(longLivedToken.AccessToken);
            var matchingPage = pageAccessTokenResponse.Data.FirstOrDefault(x => x.Name.Contains(pageName, StringComparison.OrdinalIgnoreCase));
            // Prepare SocialMediaSettings data to save
            var settings = new SocialMediaSettings
            {
                SocialOwner = socialOwner,
                Socialpagename = pageName,
                Appid = matchingPage.Id,
                Appsecret = appSecret,
                UserAccessToken = longLivedToken.AccessToken,
                PageAccessToken = pageAccessTokenResponse.Data.FirstOrDefault()?.AccessToken,
                PageId = pageAccessTokenResponse.Data.FirstOrDefault()?.Id,
                PageType = pageType,
                CreatedBy = createdBy,
                ModifiedBy = createdBy, // Assuming the same user who created it is modifying
                DateCreated = DateTime.UtcNow,
                DateModified = DateTime.UtcNow
            };

            // Save to database
            await bl.Registersystemsocialmediapagedata(JsonConvert.SerializeObject(settings));

            // Return the tokens
            return Ok(new
            {
                UserAccessToken = longLivedToken.AccessToken,
                PageAccessToken = pageAccessTokenResponse.Data.FirstOrDefault()?.AccessToken
            });
        }

        private async Task<string> GetShortLivedAccessTokenAsync(string code, string appId, string appSecret)
        {
            var requestUri = $"https://graph.facebook.com/v11.0/oauth/access_token?client_id={appId}&redirect_uri=https://yourapp.com/api/facebook/callback&client_secret={appSecret}&code={code}";

            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.GetAsync(requestUri);
                response.EnsureSuccessStatusCode();
                var responseBody = await response.Content.ReadAsStringAsync();

                var tokenResponse = JsonSerializer.Deserialize<FacebookTokenResponse>(responseBody);
                return tokenResponse.AccessToken;
            }
        }
    }
}
