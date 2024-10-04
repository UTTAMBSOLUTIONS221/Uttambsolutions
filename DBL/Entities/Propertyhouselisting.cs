namespace DBL.Entities
{
    public class Propertyhouselisting
    {
        public int Houselistingid { get; set; }
        public string? Title { get; set; }
        public decimal Price { get; set; }
        public string? Locations { get; set; }
        public bool Isforrent { get; set; }
        public int Bedrooms { get; set; }
        public int Bathrooms { get; set; }
        public string? Descriptions { get; set; }
        public string? Imageurl { get; set; }
        public string? Contacts { get; set; }
        public bool Isactive { get; set; }
        public bool Isdeleted { get; set; }
        public int Createdby { get; set; }
        public int Modifiedby { get; set; }
        public DateTime Datemodified { get; set; }
        public DateTime Datecreated { get; set; }
        public List<Propertyhouseimagesdata>? Propertyhouseimages { get; set; }
    }
    public class Propertyhouseimagesdata
    {
        public string? Propertyhouseimageurl { get; set; }
    }
}
