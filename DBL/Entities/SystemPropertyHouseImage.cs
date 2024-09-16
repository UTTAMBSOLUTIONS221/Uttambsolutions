namespace DBL.Entities
{
    public class SystemPropertyHouseImage
    {
        public long Propertyhouseid { get; set; }
        public List<SystemPropertyHouseImageModel>? PropertyHouseImage { get; set; }
    }
    public class SystemPropertyHouseImageModel
    {
        public long Propertyimageid { get; set; }
        public long Propertyhouseid { get; set; }
        public string? Houseorroom { get; set; }
        public string? Houseorroomimageurl { get; set; }
        public long Createdby { get; set; }
        public DateTime Datecreated { get; set; }
    }
}
