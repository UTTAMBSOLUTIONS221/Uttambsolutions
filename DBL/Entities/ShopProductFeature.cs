namespace DBL.Entities
{
    public class ShopProductFeature
    {
        public int ProductFeatureId { get; set; }
        public int ShopProductId { get; set; }
        public string? ProductFeature { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
