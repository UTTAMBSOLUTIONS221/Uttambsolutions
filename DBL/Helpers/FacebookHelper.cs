using DBL.Models;
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
    }
}
