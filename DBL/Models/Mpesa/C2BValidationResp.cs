using Newtonsoft.Json;

namespace DBL.Models.Mpesa
{
    public class C2BValidationResp
    {
        [JsonProperty("ResultCode")]
        public int ResultCode { get; set; }

        [JsonProperty("ResultDesc")]
        public string ResultDesc { get; set; }

        [JsonProperty("ThirdPartyTransID")]
        public string ThirdPartyTransID { get; set; }
    }
}
