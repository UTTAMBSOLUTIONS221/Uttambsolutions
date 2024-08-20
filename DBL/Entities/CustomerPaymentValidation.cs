namespace DBL.Entities
{
    public class CustomerPaymentValidation
    {
        public long CustomerPaymentId { get; set; }
        public string? TransactionReference { get; set; }
        public decimal Paidamount { get; set; }
        public decimal Actualamount { get; set; }
    }
}
