namespace DBL.Entities
{
    public class SystemPropertyHouseImage
    {
        public long Propertyimageid { get; set; }
        public long Propertyhouseid { get; set; }
        public string? Houseorroom { get; set; }
        public string? Houseorroomimageurl { get; set; }
        public long Createdby { get; set; }
        public DateTime Datecreated { get; set; }
        public List<SystemPropertyHouseImage>? PropertyHouseImage { get; set; }
    }
}
