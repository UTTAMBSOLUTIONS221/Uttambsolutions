namespace DBL.Entities
{
    public class Systemproperty
    {
        public long Propertyid { get; set; }
        public long Propertyowner { get; set; }
        public long Propertyposter { get; set; }
        public string? Propertyname { get; set; }
        public long Countyid { get; set; }
        public long Subcountyid { get; set; }
        public long Subcountywardid { get; set; }
        public string? Streetorlandmark { get; set; }
        public bool Hasdeposit { get; set; }
        public long Createdby { get; set; }
        public long Modifiedby { get; set; }
        public DateTime Datecreated { get; set; }
        public DateTime Datemodified { get; set; }
        public List<Systemhousebenefits> Housebenefits { get; set; }
    }


    public class Systemhousebenefits
    {
        public long Housebenefitid { get; set; }
        public string? Housebenefitname { get; set; }
    }
}
