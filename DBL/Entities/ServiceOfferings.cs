namespace DBL.Entities
{
    public class ServiceOfferings
    {
        public int Staffserviceid { get; set; }
        public int Staffid { get; set; }
        public int Servicetypeid { get; set; }
        public string? Servicedescription { get; set; }
        public int Coutyid { get; set; }
        public int Subcountyid { get; set; }
        public int Subcountywardid { get; set; }
        public string? Housename { get; set; }
        public string? Contacts { get; set; }
        public bool Isactive { get; set; }
        public bool Isdeleted { get; set; }
    }
}
