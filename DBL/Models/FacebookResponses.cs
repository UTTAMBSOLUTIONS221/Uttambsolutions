using Newtonsoft.Json;

namespace DBL.Models
{
    public class FacebookTokenResponse
    {
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }

        [JsonProperty("token_type")]
        public string TokenType { get; set; }

        [JsonProperty("expires_in")]
        public int ExpiresIn { get; set; }
    }

    public class FacebookExchangeTokenResponse
    {

        [JsonProperty("access_token")]
        public string AccessToken { get; set; }

        [JsonProperty("token_type")]
        public string TokenType { get; set; }

        [JsonProperty("expires_in")]
        public int ExpiresIn { get; set; }
    }

    public class FacebookNeverExpiresResponse
    {
        public List<FacebookAccountData> Data { get; set; }
    }

    public class FacebookAccountData
    {
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }
        public string Category { get; set; }
        public string Name { get; set; }
        public string Id { get; set; }
    }
}
