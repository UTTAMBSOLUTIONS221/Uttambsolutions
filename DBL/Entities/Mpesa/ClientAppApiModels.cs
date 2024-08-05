using Newtonsoft.Json;

namespace DBL.Entities.Mpesa
{
    public class PesaAppRequestData
    {
        [JsonProperty("tsp")]
        public string TimeStamp { get; set; }

        [JsonProperty("scode")]
        public int ServiceCode { get; set; }

        [JsonProperty("acode")]
        public int AppCode { get; set; }

        [JsonProperty("data")]
        public dynamic Data { get; set; }
    }

    public class MakePaymentData
    {
        [JsonProperty("batch")]
        public string BatchNo { get; set; }

        [JsonProperty("payments")]
        public List<PaymentRecord> Payments { get; set; }

        public MakePaymentData()
        {
            Payments = new List<PaymentRecord>();
        }
    }

    public class PaymentRecord
    {
        [JsonProperty("accno")]
        public string AccountNo { get; set; }

        [JsonProperty("amount")]
        public decimal Amount { get; set; }

        [JsonProperty("refno")]
        public string RefNo { get; set; }

        [JsonProperty("accref")]
        public string AccountRef { get; set; }
    }
}
