namespace DBL.Entities
{
    public class Systemservices
    {
        public int Serviceid { get; set; }
        public string? Servicename { get; set; }
        public decimal Subscriptionfee { get; set; }
        public bool Isvisible { get; set; }
        public List<Systemservicesitems>? Serviceitems { get; set; }
        public List<Systemservicesitems>? Data { get; set; }
    }
    public class Systemservicesitems
    {
        public int Serviceitemid { get; set; }
        public int Serviceid { get; set; }
        public string? Serviceitemname { get; set; }
        public string? Serviceitemimageurl { get; set; }

        public int Staffserviceid { get; set; }
        public int Servicetypeitemid { get; set; }
        public decimal Servicefee { get; set; }
        public bool Isfixed { get; set; }
    }
}
