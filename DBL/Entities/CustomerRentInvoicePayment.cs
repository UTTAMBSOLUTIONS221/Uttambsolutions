namespace DBL.Entities
{
    public class CustomerRentInvoicePayment
    {
        public long Customerpaymentid { get; set; }
        public long Tenantid { get; set; }
        public long Houseromid { get; set; }
        public long Paymentmodeid { get; set; }
        public long Financetransactionid { get; set; }
        public decimal Amount { get; set; }
        public string? Transactionreference { get; set; }
        public DateTime Transactiondate { get; set; }
        public bool Ispaymentvalidated { get; set; }
        public string? Chequeno { get; set; }
        public DateTime Chequedate { get; set; }
        public string? Memo { get; set; }
        public string? Drawerbank { get; set; }
        public string? Depositbank { get; set; }
        public long Paidby { get; set; }
        public string? Slipreference { get; set; }
        public DateTime Datecreated { get; set; }
    }
}
