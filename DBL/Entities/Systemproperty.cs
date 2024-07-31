namespace DBL.Entities
{
    public class Systemproperty
    {
        public long Propertyid { get; set; }
        public long Propertyowner { get; set; }
        public long Propertyposter { get; set; }
        public string? Propertyname { get; set; }
        public long Createdby { get; set; }
        public long Modifiedby { get; set; }
        public DateTime Datecreated { get; set; }
        public DateTime Datemodified { get; set; }
    }
}
