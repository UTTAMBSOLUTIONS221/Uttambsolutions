﻿namespace DBL.Models
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
        public string Sentstatus => Issent ? "Sent" : "Not Sent";
        public string? Paidstatus { get; set; }
    }

}
