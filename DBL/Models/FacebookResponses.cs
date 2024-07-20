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

        [JsonProperty("scope")]
        public string Scope { get; set; }
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
        [JsonProperty("data")]
        public List<FacebookAccountData> Data { get; set; }
    }

    public class FacebookAccountData
    {
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }

        [JsonProperty("category")]
        public string Category { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }
    }

    public class FacebookUserProfile
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }
    }

    public class FacebookPageInsights
    {
        [JsonProperty("data")]
        public List<FacebookPageInsightData> Data { get; set; }
    }

    public class FacebookPageInsightData
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("period")]
        public string Period { get; set; }

        [JsonProperty("values")]
        public List<FacebookPageInsightValue> Values { get; set; }
    }

    public class FacebookPageInsightValue
    {
        [JsonProperty("value")]
        public int Value { get; set; }

        [JsonProperty("end_time")]
        public string EndTime { get; set; }
    }
}
