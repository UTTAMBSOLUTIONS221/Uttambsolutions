using Newtonsoft.Json;

namespace DBL.Models.Mpesa
{
    public class ThirdPartyPaymentResponse
    {
        [JsonProperty("stat")]
        public int Status { get; set; }

        [JsonProperty("msg")]
        public string Message { get; set; }
    }
}
