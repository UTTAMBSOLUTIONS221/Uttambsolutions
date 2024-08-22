namespace DBL.Entities
{
    public class CustomerPaymentValidation
    {
        public long CustomerPaymentId { get; set; }
        public long Houseroomid { get; set; }
        public long Tenantid { get; set; }
        public long Confirmedby { get; set; }
        public decimal Actualamount { get; set; }
        public DateTime Datecreated { get; set; }
        public DateTime Datemodified { get; set; }
    }
}
