namespace DBL.Models
{
    public class TenantMonthlyInvoiceData
    {
        public List<MonthlyRentInvoiceModel>? Data { get; set; }
    }
    public class TenantMonthlyInvoiceDetailData
    {
        public MonthlyRentInvoiceModel? Data { get; set; }
    }
    public class MonthlyRentInvoiceModel
    {
        public int Invoiceid { get; set; }
        public int Financetransactionid { get; set; }
        public string? Transactioncode { get; set; }
        public string? Invoiceno { get; set; }
        public int Propertyhouseroomid { get; set; }
        public string? Systemhousesizename { get; set; }
        public string? Systempropertyhousesizename { get; set; }
        public int Propertyhouseroomtenantid { get; set; }
        public string? Tenantname { get; set; }
        public DateTime Datecreated { get; set; }
        public DateTime Duedate { get; set; }
        public decimal Amount { get; set; }
        public decimal Discount { get; set; }
        public decimal Balance { get; set; }
        public bool Ispaid { get; set; }
        public decimal Paidamount { get; set; }
        public bool Issent { get; set; }
        public string Sentstatus => Issent ? "Notification Sent" : "Notification Not Sent";
        public string? Paidstatus { get; set; }
        public List<MonthlyRentInvoiceItem>? InvoiceDetails { get; set; }
    }

    public class MonthlyRentInvoiceItem
    {
        public int Invoiceitemid { get; set; }
        public int Invoiceid { get; set; }
        public int Systempropertyhousedepositfeeid { get; set; }
        public string? Housedepositfeename { get; set; }
        public decimal Units { get; set; }
        public decimal Price { get; set; }
        public decimal Discount { get; set; }
    }
}
