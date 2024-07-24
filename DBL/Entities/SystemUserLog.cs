namespace DBL.Entities
{
    public class SystemUserLog
    {
        public int Logid { get; set; }
        public int Userid { get; set; }
        public string? Logaction { get; set; }
        public string? Browser { get; set; }
        public string? Ipaddress { get; set; }
        public decimal Loyaltyreward { get; set; }
        public int Loyaltystatus { get; set; }
        public int LogactionexitTime { get; set; }
        public DateTime Datecreated { get; set; }
    }
}
