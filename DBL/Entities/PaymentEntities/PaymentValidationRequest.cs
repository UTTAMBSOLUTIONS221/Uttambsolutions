namespace DBL.Entities.PaymentEntities
{
    public class PaymentValidationRequest
    {
        public string? BillerCode { get; set; }
        public string? CustomerRefNumber { get; set; }
        public decimal Amount { get; set; }
        public string? AmountCurrency { get; set; }
        public string? CcountryCode { get; set; }
    }
}
