﻿using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Jobs.Controllers
{
    [Route("linkedin")]
    public class LinkedInController : Controller
    {
        private readonly HttpClient _client;

        public LinkedInController()
        {
            _client = new HttpClient();
        }

        // Endpoint to generate and redirect to LinkedIn's authorization URL
        [HttpGet("redirect")]
        public IActionResult RedirectToLinkedIn()
        {
            string clientId = "7797zie5ixisk8";
            string redirectUri = "https://academicresearchwriters.uttambsolutions.com/linkedin/callback";
            string state = "randomState"; // Generate a random or securely generated state string for each request

            var authorizationUrl = GetAuthorizationUrl(clientId, redirectUri, state);
            return Redirect(authorizationUrl);
        }

        private string GetAuthorizationUrl(string clientId, string redirectUri, string state)
        {
            return $"https://www.linkedin.com/oauth/v2/authorization?response_type=code&client_id={clientId}&redirect_uri={Uri.EscapeDataString(redirectUri)}&state={state}&scope=w_member_social";
        }

        [HttpGet("callback")]
        public async Task<IActionResult> LinkedInCallback(string code, string state)
        {
            string clientId = "7797zie5ixisk8";
            string clientSecret = "OjkMPRqTXU78vbw7";
            string redirectUri = "https://academicresearchwriters.uttambsolutions.com/linkedin/callback";

            if (string.IsNullOrEmpty(code))
            {
                return BadRequest("Authorization code is missing.");
            }

            try
            {
                var accessToken = await GetAccessTokenAsync(clientId, clientSecret, redirectUri, code);
                // Use the access token to post jobs or other actions
                return Ok("Authorization successful. Access token: " + accessToken);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error exchanging authorization code: {ex.Message}");
            }
        }

        // Method to exchange the authorization code for an access token
        private async Task<string> GetAccessTokenAsync(string clientId, string clientSecret, string redirectUri, string authCode)
        {
            var requestBody = new Dictionary<string, string>
            {
                { "grant_type", "authorization_code" },
                { "code", authCode },
                { "redirect_uri", redirectUri },
                { "client_id", clientId },
                { "client_secret", clientSecret }
            };

            var requestContent = new FormUrlEncodedContent(requestBody);
            var response = await _client.PostAsync("https://www.linkedin.com/oauth/v2/accessToken", requestContent);

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException($"Error fetching access token: {errorContent}");
            }

            var responseContent = await response.Content.ReadAsStringAsync();
            var tokenResponse = JsonConvert.DeserializeObject<TokenResponse>(responseContent);

            return tokenResponse.AccessToken;
        }

        public class TokenResponse
        {
            [JsonProperty("access_token")]
            public string AccessToken { get; set; }
        }
    }
}