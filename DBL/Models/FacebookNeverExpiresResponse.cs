using System.Text.Json.Serialization;

namespace DBL.Models
{
    public class FacebookNeverExpiresResponse
    {
        [JsonPropertyName("data")]
        public List<PageData> Data { get; set; }

        [JsonPropertyName("paging")]
        public Paging Paging { get; set; }
    }
    public class PageData
    {
        [JsonPropertyName("access_token")]
        public string AccessToken { get; set; }

        [JsonPropertyName("category")]
        public string Category { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("perms")]
        public List<string> Permissions { get; set; }
    }

    public class Paging
    {
        [JsonPropertyName("cursors")]
        public Cursors Cursors { get; set; }
    }

    public class Cursors
    {
        [JsonPropertyName("before")]
        public string Before { get; set; }

        [JsonPropertyName("after")]
        public string After { get; set; }
    }
}
