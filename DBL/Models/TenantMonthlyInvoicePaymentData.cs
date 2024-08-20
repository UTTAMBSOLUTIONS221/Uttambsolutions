namespace DBL.Models
{
    public class TenantMonthlyInvoicePaymentData
    {
        public List<CustomerPaymentData>? Data { get; set; }
    }
    public class CustomerPaymentData
    {
        public int CustomerPaymentId { get; set; }
        public int HouseRoomTenantId { get; set; }
        public string? HouseTenantName { get; set; }
        public int HouseRoomId { get; set; }
        public string? HouseOwnerName { get; set; }
        public string? PropertyHouseName { get; set; }
        public string? SystemPropertyHouseSizeName { get; set; }
        public string? SystemHouseSizeName { get; set; }
        public string? SystemHouseRoomSizeName
        {
            get
            {
                // Ensure that both properties are not null before concatenating
                if (!string.IsNullOrEmpty(SystemPropertyHouseSizeName) && !string.IsNullOrEmpty(SystemHouseSizeName))
                {
                    return $"{SystemPropertyHouseSizeName} - {SystemHouseSizeName}";
                }
                // Handle cases where one or both properties might be null or empty
                else if (!string.IsNullOrEmpty(SystemPropertyHouseSizeName))
                {
                    return SystemPropertyHouseSizeName;
                }
                else if (!string.IsNullOrEmpty(SystemHouseSizeName))
                {
                    return SystemHouseSizeName;
                }
                else
                {
                    return string.Empty;
                }
            }
        }
        public int PaymentModeId { get; set; }
        public string? PaymentMode { get; set; }
        public int FinanceTransactionId { get; set; }
        public string? TransactionCode { get; set; }
        public decimal Amount { get; set; }
        public string? TransactionReference { get; set; }
        public DateTime TransactionDate { get; set; }
        public bool IsPaymentValidated { get; set; }
        public string PaymentValidationstatus => IsPaymentValidated ? "Validated" : "Not Validated";
        public string? ChequeNo { get; set; }
        public DateTime? ChequeDate { get; set; }
        public string? Memo { get; set; }
        public string? DrawerBank { get; set; }
        public string? DepositBank { get; set; }
        public string? PaidBy { get; set; }
        public string? SlipReference { get; set; }
        public DateTime DateCreated { get; set; }
    }

}
