using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace WEB.Controllers
{
    public class LinkedInController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;

        public LinkedInController(IConfiguration configuration, HttpClient httpClient)
        {
            _configuration = configuration;
            _httpClient = httpClient;
        }

        [HttpGet("linkedin/authorize")]
        public IActionResult Authorize()
        {
            string clientId = _configuration["LinkedIn:ClientId"];
            string redirectUri = _configuration["LinkedIn:RedirectUri"];
            string scope = "r_liteprofile r_emailaddress w_member_social"; // Adjust scopes as needed

            string authorizationUrl = $"https://www.linkedin.com/oauth/v2/authorization?response_type=code&client_id={clientId}&redirect_uri={redirectUri}&scope={scope}";

            return Redirect(authorizationUrl); // Redirects the user to LinkedIn for authentication
        }

        [HttpGet("linkedin/callback")]
        public async Task<IActionResult> Callback(string code)
        {
            if (string.IsNullOrEmpty(code))
            {
                ViewBag.Error = "Authorization code not received";
                return View("Error"); // Redirect to an error page
            }

            try
            {
                string clientId = _configuration["LinkedIn:ClientId"];
                string clientSecret = _configuration["LinkedIn:ClientSecret"];
                string redirectUri = _configuration["LinkedIn:RedirectUri"];

                var tokenRequest = new HttpRequestMessage(HttpMethod.Post, "https://www.linkedin.com/oauth/v2/accessToken")
                {
                    Content = new FormUrlEncodedContent(new[]
                    {
                    new KeyValuePair<string, string>("grant_type", "authorization_code"),
                    new KeyValuePair<string, string>("code", code),
                    new KeyValuePair<string, string>("redirect_uri", redirectUri),
                    new KeyValuePair<string, string>("client_id", clientId),
                    new KeyValuePair<string, string>("client_secret", clientSecret)
                })
                };

                HttpResponseMessage tokenResponse = await _httpClient.SendAsync(tokenRequest);
                tokenResponse.EnsureSuccessStatusCode();

                string responseContent = await tokenResponse.Content.ReadAsStringAsync();
                JObject json = JObject.Parse(responseContent);
                string accessToken = json["access_token"].ToString();

                // Store the token in session or database as needed
                HttpContext.Session.SetString("LinkedInAccessToken", accessToken);

                ViewBag.Message = "Access token retrieved successfully!";
                return View("Success");
            }
            catch (Exception ex)
            {
                ViewBag.Error = $"Error retrieving access token: {ex.Message}";
                return View("Error");
            }
        }
    }
}
