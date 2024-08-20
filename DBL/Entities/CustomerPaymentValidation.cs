namespace DBL.Entities
{
    public class CustomerPaymentValidation
    {
        public long CustomerPaymentId { get; set; }
        public long FinanceTransactionId { get; set; }
        public long Houseroomid { get; set; }
        public long Tenantid { get; set; }
        public string? TransactionReference { get; set; }
        public decimal Paidamount { get; set; }
        public decimal Actualamount { get; set; }
        public DateTime Datecreated { get; set; }
        public DateTime Datemodified { get; set; }
    }
}
