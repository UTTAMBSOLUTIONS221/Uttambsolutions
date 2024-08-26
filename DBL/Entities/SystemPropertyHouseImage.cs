namespace DBL.Entities
{
    public class SystemPropertyHouseImage
    {
        public long PropertyImageId { get; set; }
        public long PropertyHouseId { get; set; }
        public string? HouseOrRoom { get; set; }
        public string? SignatureImageUrl { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
