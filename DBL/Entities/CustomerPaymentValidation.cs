namespace DBL.Entities
{
    public class CustomerPaymentValidation
    {
        public int CustomerPaymentId { get; set; }
        public int HouseRoomTenantId { get; set; }
        public int Houseroomid { get; set; }
        public int PaymentModeId { get; set; }
        public int Financetransactionid { get; set; }
        public long Tenantid { get; set; }
        public long Confirmedby { get; set; }
        public decimal Amount { get; set; }
        public decimal Actualamount { get; set; }
        public string? TransactionReference { get; set; }
        public DateTime TransactionDate { get; set; }
        public bool IsPaymentValidated { get; set; }
        public string? ChequeNo { get; set; }
        public DateTime? ChequeDate { get; set; }
        public string? Memo { get; set; }
        public string? DrawerBank { get; set; }
        public string? DepositBank { get; set; }
        public int PaidBy { get; set; }
        public int ValidatedBy { get; set; }
        public string? SlipReference { get; set; }
        public DateTime Datecreated { get; set; }
        public DateTime Datemodified { get; set; }
    }
}
