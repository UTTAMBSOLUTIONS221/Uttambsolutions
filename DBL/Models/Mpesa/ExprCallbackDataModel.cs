namespace DBL.Models.Mpesa
{
    public class ExprCallbackDataModel
    {
        public string CheckoutRequestID { get; set; }
        public int ResultCode { get; set; }
        public string ResultDesc { get; set; }
        public string RefNo { get; set; }
        public string CustomerDets { get; set; }
        public string TxnDate { get; set; }
        public string PhoneNo { get; set; }
        public decimal Amount { get; set; }
        public decimal Balance { get; set; }
    }
}
