using Newtonsoft.Json;

namespace DBL.Models.Mpesa
{
    public class PayResponse
    {
        [JsonProperty("stat")]
        public int Status { get; set; }

        [JsonProperty("msg")]
        public string Message { get; set; }

        [JsonProperty("data")]
        public dynamic Data { get; set; }
    }

    public class PaymentResponseRecord
    {
        [JsonProperty("stat")]
        public int Status { get; set; }

        [JsonProperty("msg")]
        public string Message { get; set; }

        [JsonProperty("accno")]
        public string AccountNo { get; set; }

        [JsonProperty("refno")]
        public string RefNo { get; set; }
    }
}
