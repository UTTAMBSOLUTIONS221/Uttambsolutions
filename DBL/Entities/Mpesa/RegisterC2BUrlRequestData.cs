using Newtonsoft.Json;

namespace DBL.Entities.Mpesa
{
    public class RegisterC2BUrlRequestData
    {
        [JsonProperty("ShortCode")]
        public string ShortCode { get; set; }

        [JsonProperty("ResponseType")]
        public string ResponseType { get; set; }

        [JsonProperty("ConfirmationURL")]
        public string ConfirmationURL { get; set; }

        [JsonProperty("ValidationURL")]
        public string ValidationURL { get; set; }
    }
}
