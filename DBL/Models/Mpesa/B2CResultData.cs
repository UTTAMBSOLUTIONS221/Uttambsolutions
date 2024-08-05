namespace DBL.Models.Mpesa
{
    public class B2CResultData
    {
        public string ResultCode { get; set; }
        public string ResultDescr { get; set; }
        public string TxnID { get; set; }
        public string OrgRef { get; set; }
        public decimal TxnAmount { get; set; }
        public decimal WorkingBalance { get; set; }
        public decimal UtilityBalance { get; set; }
        public string Receiver { get; set; }
        public string ReceiverReg { get; set; }
        public decimal Charge { get; set; }
    }
}
