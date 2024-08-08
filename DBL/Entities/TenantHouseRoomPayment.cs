namespace DBL.Entities
{
    public class TenantHouseRoomPayment
    {
        public long TenanthousepaymentId { get; set; }
        public long TenantId { get; set; }
        public long TenanthouserroomId { get; set; }
        public long PaymentModeId { get; set; }
        public long FinanceTransactionId { get; set; }
        public decimal Amount { get; set; }
        public string? TransactionReference { get; set; }
        public DateTime TransactionDate { get; set; }
        public long CreatedBy { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
