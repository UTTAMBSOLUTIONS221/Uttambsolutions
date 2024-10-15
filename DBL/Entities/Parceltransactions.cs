namespace DBL.Entities
{
    public class Parceltransactions
    {
        public int Transactionid { get; set; }
        public int Parcelid { get; set; }
        public decimal Amount { get; set; }
        public int Paymentmethodid { get; set; }
        public DateTime Transactiondate { get; set; }
        public int Createdby { get; set; }
    }
}
