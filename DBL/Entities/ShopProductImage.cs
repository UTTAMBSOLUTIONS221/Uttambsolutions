namespace DBL.Entities
{
    public class ShopProductImage
    {
        public int ProductImagesId { get; set; }
        public int ShopProductId { get; set; }
        public string? ProductImageUrl { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
