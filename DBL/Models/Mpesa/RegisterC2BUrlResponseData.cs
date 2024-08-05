using Newtonsoft.Json;

namespace DBL.Models.Mpesa
{
    public class RegisterC2BUrlResponseData
    {
        [JsonProperty("ConversationID")]
        public string ConversationID { get; set; }

        [JsonProperty("OriginatorConversationID")]
        public string OriginatorConversationID { get; set; }

        [JsonProperty("ResponseDescription")]
        public string ResponseDescription { get; set; }
    }
}
