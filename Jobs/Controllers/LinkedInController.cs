﻿using DBL;
using Jobs;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

[Route("linkedin")]
public class LinkedInController : Controller
{
    private readonly IConfiguration _configuration;
    private readonly HttpClient _httpClient = new HttpClient();
    private readonly BL bl;
    public LinkedInController(IConfiguration config)
    {
        bl = new BL(Util.ShareConnectionString(config));
    }


    [HttpGet("redirect")]
    public async Task<IActionResult> RedirectToLinkedIn()
    {

        var authorizationUrl = "";
        string redirectUri = "https://academicresearchwriters.uttambsolutions.com/linkedin/callback";
        //get social media apps to update the access tokens
        var linkedinapps = await bl.Getsystemalllinkedinsocialmediadata();
        if (linkedinapps != null)
        {
            foreach (var app in linkedinapps)
            {
                string state = app.PageId;
                authorizationUrl = $"https://www.linkedin.com/oauth/v2/authorization?response_type=code&client_id={app.Appid}&redirect_uri={Uri.EscapeDataString(redirectUri)}&state={state}&scope=w_member_social";
            }
        }
        return Redirect(authorizationUrl);
    }

    [HttpGet("callback")]
    public async Task<IActionResult> LinkedInCallback(string code, string state)
    {
        if (string.IsNullOrEmpty(code))
        {
            return BadRequest("Authorization code is missing.");
        }

        try
        {
            var app = await bl.GetLinkedinSocialMediaDataById(state);
            if (app == null)
            {
                return BadRequest("LinkedIn app not found.");
            }

            // Exchange authorization code for access token
            var tokenResponse = await ExchangeCodeForTokenAsync(app, clientSecret, redirectUri, code);

            // Save the access token and refresh token in the database with the associated LinkedIn app
            await SaveTokensToDatabaseAsync(app.Appid, tokenResponse.AccessToken, tokenResponse.RefreshToken, tokenResponse.ExpiresIn);

            return Ok("Job posted successfully on LinkedIn.");
        }
        catch (Exception ex)
        {
            return BadRequest($"Error exchanging authorization code: {ex.Message}");
        }
    }


    [HttpGet("callback")]
    public async Task<IActionResult> LinkedInCallback(string code, string state)
    {
        if (string.IsNullOrEmpty(code))
        {
            return BadRequest("Authorization code is missing.");
        }

        string clientId = _configuration["LinkedIn:ClientId"];
        string clientSecret = _configuration["LinkedIn:ClientSecret"];
        string redirectUri = _configuration["LinkedIn:RedirectUri"];

        try
        {
            // Exchange authorization code for access token
            var tokenResponse = await ExchangeCodeForTokenAsync(clientId, clientSecret, redirectUri, code);

            // Save the access token and refresh token in the database with an associated LinkedIn page
            await SaveTokensToDatabaseAsync(clientId, tokenResponse.AccessToken, tokenResponse.RefreshToken, tokenResponse.ExpiresIn);

            return Ok("Job posted successfully on LinkedIn.");
        }
        catch (Exception ex)
        {
            return BadRequest($"Error exchanging authorization code: {ex.Message}");
        }
    }

    [HttpGet("refresh")]
    public async Task<IActionResult> RefreshAccessToken(string clientId)
    {
        var clientSecret = _configuration["LinkedIn:ClientSecret"];

        try
        {
            // Retrieve refresh token from database based on clientId
            var refreshToken = await GetRefreshTokenFromDatabaseAsync(clientId);

            if (string.IsNullOrEmpty(refreshToken))
            {
                return BadRequest("Refresh token not found or expired. Reauthorize.");
            }

            // Exchange refresh token for a new access token
            var accessToken = await RefreshAccessTokenAsync(clientId, clientSecret, refreshToken);

            // Update access token in database
            await UpdateAccessTokenInDatabaseAsync(clientId, accessToken);

            return Ok("Access token refreshed successfully.");
        }
        catch (Exception ex)
        {
            return BadRequest($"Error refreshing access token: {ex.Message}");
        }
    }

    private async Task<TokenResponse> ExchangeCodeForTokenAsync(string clientId, string clientSecret, string redirectUri, string authCode)
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
        var response = await _httpClient.PostAsync("https://www.linkedin.com/oauth/v2/accessToken", requestContent);

        if (!response.IsSuccessStatusCode)
        {
            var errorContent = await response.Content.ReadAsStringAsync();
            throw new Exception($"Failed to retrieve access token: {errorContent}");
        }

        var responseContent = await response.Content.ReadAsStringAsync();
        var tokenResponse = JsonConvert.DeserializeObject<TokenResponse>(responseContent);

        return tokenResponse;
    }

    private async Task<string> RefreshAccessTokenAsync(string clientId, string clientSecret, string refreshToken)
    {
        var requestBody = new Dictionary<string, string>
        {
            { "grant_type", "refresh_token" },
            { "refresh_token", refreshToken },
            { "client_id", clientId },
            { "client_secret", clientSecret }
        };

        var requestContent = new FormUrlEncodedContent(requestBody);
        var response = await _httpClient.PostAsync("https://www.linkedin.com/oauth/v2/accessToken", requestContent);

        if (!response.IsSuccessStatusCode)
        {
            var errorContent = await response.Content.ReadAsStringAsync();
            throw new Exception($"Failed to refresh access token: {errorContent}");
        }

        var responseContent = await response.Content.ReadAsStringAsync();
        var tokenResponse = JsonConvert.DeserializeObject<TokenResponse>(responseContent);

        return tokenResponse.AccessToken;
    }

    private async Task SaveTokensToDatabaseAsync(string clientId, string accessToken, string refreshToken, int expiresIn)
    {
        var existingToken = await _dbContext.LinkedInTokens.FindAsync(clientId);

        if (existingToken != null)
        {
            existingToken.AccessToken = accessToken;
            existingToken.RefreshToken = refreshToken;
            existingToken.ExpiryDate = DateTime.UtcNow.AddSeconds(expiresIn); // Set token expiration
        }
        else
        {
            _dbContext.LinkedInTokens.Add(new LinkedInToken
            {
                ClientId = clientId,
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                ExpiryDate = DateTime.UtcNow.AddSeconds(expiresIn) // Set token expiration
            });
        }

        await _dbContext.SaveChangesAsync();
    }

    private async Task<string> GetRefreshTokenFromDatabaseAsync(string clientId)
    {
        var token = await _dbContext.LinkedInTokens.FindAsync(clientId);
        return token?.RefreshToken;
    }

    private async Task UpdateAccessTokenInDatabaseAsync(string clientId, string accessToken)
    {
        var token = await _dbContext.LinkedInTokens.FindAsync(clientId);

        if (token != null)
        {
            token.AccessToken = accessToken;
            token.ExpiryDate = DateTime.UtcNow.AddHours(24); // Example: Extend token validity
            await _dbContext.SaveChangesAsync();
        }
    }

    public class TokenResponse
    {
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }

        [JsonProperty("expires_in")]
        public int ExpiresIn { get; set; }

        [JsonProperty("refresh_token")]
        public string RefreshToken { get; set; }
    }
}