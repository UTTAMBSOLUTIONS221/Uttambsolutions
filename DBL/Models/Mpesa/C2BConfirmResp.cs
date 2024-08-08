using Newtonsoft.Json;

namespace DBL.Models.Mpesa
{
    public class C2BConfirmResp
    {
        [JsonProperty("ResultCode")]
        public int ResultCode { get; set; }

        [JsonProperty("ResultDesc")]
        public string ResultDesc { get; set; }
    }
}
