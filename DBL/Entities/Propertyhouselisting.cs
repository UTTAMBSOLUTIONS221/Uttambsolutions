namespace DBL.Entities
{
    public class Propertyhouselisting
    {
        public int Houselistingid { get; set; }
        public string? Title { get; set; }
        public decimal Price { get; set; }
        public string? Location { get; set; }
        public bool Isforrent { get; set; }
        public int Bedrooms { get; set; }
        public int Bathrooms { get; set; }
        public string? Description { get; set; }
        public string? Imageurl { get; set; }
        public string? Contacts { get; set; }
        public List<Propertyhouseimagesdata>? Propertyhouseimages { get; set; }
    }
    public class Propertyhouseimagesdata
    {
        public string? Propertyhouseimageurl { get; set; }
    }
}
