using Newtonsoft.Json;

namespace DBL.Entities.Mpesa
{
    public class PaymentNotificationData
    {
        [JsonProperty("amnt")]
        public decimal Amount { get; set; }

        [JsonProperty("refno")]
        public string ReferenceNo { get; set; }

        [JsonProperty("accno")]
        public string PayAccountNo { get; set; }

        [JsonProperty("custno")]
        public string CustomerNo { get; set; }

        [JsonProperty("custname")]
        public string CustomerName { get; set; }

        [JsonProperty("bal")]
        public decimal AccountBalance { get; set; }

        [JsonProperty("sref")]
        public string SourceRef { get; set; }
    }
}
