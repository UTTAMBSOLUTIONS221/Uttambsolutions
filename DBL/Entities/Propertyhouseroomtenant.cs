namespace DBL.Entities
{
    public class Propertyhouseroomtenant
    {
        public long Houseroomid { get; set; }
        public long Tenantid { get; set; }
        public int Status { get; set; }
        public long Createdby { get; set; }
        public DateTime Datecreated { get; set; }
    }
}
