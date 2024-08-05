//using DBL;
//using DBL.Entities;
//using DBL.Helpers;
//using DBL.Models;
//using Microsoft.AspNetCore.Mvc;
//using Newtonsoft.Json;

//namespace Blog.Controllers
//{
//    [ApiController]
//    [Route("api/facebook")]
//    public class FacebookController : ControllerBase
//    {
//        private readonly FacebookHelper _facebookHelper;
//        private readonly BL bl;
//        private readonly IConfiguration _config;

//        public FacebookController(FacebookHelper facebookHelper, IConfiguration config)
//        {
//            _facebookHelper = facebookHelper;
//            bl = new BL(Util.ShareConnectionString(config));
//            _config = config;
//        }


//        [HttpGet("callback")]
//        public async Task<IActionResult> Callback([FromQuery] string code)
//        {
//            if (string.IsNullOrEmpty(code))
//            {
//                return BadRequest("Authorization code is missing.");
//            }

//            // Retrieve SocialMediaData from database or secure storage
//            var socialMediaData = HttpContext.Session.GetString("SocialMediaData");
//            var socialMediaDataObject = string.IsNullOrEmpty(socialMediaData)
//                ? null
//                : JsonConvert.DeserializeObject<SocialMediaSettings>(socialMediaData);

//            if (socialMediaDataObject == null)
//            {
//                return BadRequest("Social media data is missing.");
//            }

//            var appId = socialMediaDataObject.Appid;
//            var appSecret = socialMediaDataObject.Appsecret;

//            if (string.IsNullOrEmpty(appId) || string.IsNullOrEmpty(appSecret))
//            {
//                return BadRequest("AppId or AppSecret is missing.");
//            }

//            // Step 1: Get the short-lived access token
//            var shortLivedToken = await GetShortLivedAccessTokenAsync(code, appId, appSecret);

//            // Step 2: Exchange the short-lived token for a long-lived access token (user access token)
//            //var longLivedToken = await _facebookHelper.ExchangeAccessTokenAsync(appId, appSecret, shortLivedToken);

//            // Step 3: Generate page access tokens (if applicable)
//            //var pageAccessTokenResponse = await _facebookHelper.GenerateNeverExpiresAccessTokenAsync(longLivedToken.AccessToken);
//            //var matchingPage = pageAccessTokenResponse.Data.FirstOrDefault(x => x.Name.Contains(socialMediaDataObject?.Socialpagename, StringComparison.OrdinalIgnoreCase));

//            // Prepare SocialMediaSettings data to save
//            var settings = new SocialMediaSettings
//            {
//                SocialOwner = socialMediaDataObject?.SocialOwner ?? 0,
//                Socialpagename = socialMediaDataObject?.Socialpagename,
//                Appid = appId,
//                Appsecret = appSecret,
//                UserAccessToken = longLivedToken.AccessToken,
//                PageAccessToken = pageAccessTokenResponse.Data.FirstOrDefault()?.AccessToken,
//                PageId = matchingPage?.Id,
//                PageType = socialMediaDataObject?.PageType,
//                CreatedBy = socialMediaDataObject?.CreatedBy ?? 0,
//                ModifiedBy = socialMediaDataObject?.ModifiedBy ?? 0,
//                DateCreated = DateTime.UtcNow,
//                DateModified = DateTime.UtcNow
//            };

//            // Save to database
//            await bl.Registersystemsocialmediapagedata(JsonConvert.SerializeObject(settings));

//            // Return the tokens
//            return Ok(new
//            {
//                UserAccessToken = longLivedToken.AccessToken,
//                PageAccessToken = pageAccessTokenResponse.Data.FirstOrDefault()?.AccessToken
//            });
//        }

//        private async Task<string> GetShortLivedAccessTokenAsync(string code, string appId, string appSecret)
//        {
//            var requestUri = $"https://graph.facebook.com/v11.0/oauth/access_token?client_id={appId}&redirect_uri=https://fortysevennews.uttambsolutions.com/api/facebook/callback&client_secret={appSecret}&code={code}";

//            using (var httpClient = new HttpClient())
//            {
//                try
//                {
//                    var response = await httpClient.GetAsync(requestUri);
//                    response.EnsureSuccessStatusCode();
//                    var responseBody = await response.Content.ReadAsStringAsync();

//                    var tokenResponse = JsonConvert.DeserializeObject<FacebookTokenResponse>(responseBody);
//                    return tokenResponse?.AccessToken ?? throw new Exception("Access token not found.");
//                }
//                catch (Exception ex)
//                {
//                    // Log the exception
//                    throw new Exception("Failed to get short-lived access token.", ex);
//                }
//            }
//        }
//    }
//}
