namespace DBL.Models
{
    public class Monthlyrentinvoicedata
    {
        public List<Monthlyrentinvoice>? Data { get; set; }
    }
    public class Monthlyrentinvoice
    {
        public int InvoiceId { get; set; }
        public string? InvoiceNo { get; set; }
        public string? Propertyownername { get; set; }
        public string? Ownerphonenumber { get; set; }
        public string? FullName { get; set; }
        public string? EmailAddress { get; set; }
        public string? PhoneNumber { get; set; }
        public string? PropertyHouseName { get; set; }
        public string? SystemPropertyHouseSizeName { get; set; }
        public int PropertyHouseRoomId { get; set; }
        public int PropertyHouseRoomTenantId { get; set; }
        public int FinanceTransactionId { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DueDate { get; set; }
        public decimal Amount { get; set; }
        public decimal Discount { get; set; }
        public bool IsPaid { get; set; }
        public decimal PaidAmount { get; set; }
        public decimal Balance { get; set; }
        public bool IsSent { get; set; }
        public string? PaidStatus { get; set; }
        public List<InvoiceDetail>? InvoiceDetails { get; set; }
    }
    public class InvoiceDetail
    {
        public int InvoiceItemId { get; set; }
        public int InvoiceId { get; set; }
        public int SystemPropertyHouseDepositFeeId { get; set; }
        public string? HouseDepositFeeName { get; set; }
        public decimal Units { get; set; }
        public decimal Price { get; set; }
        public decimal Discount { get; set; }
    }

}
