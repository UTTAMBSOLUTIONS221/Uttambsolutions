namespace DBL.Entities
{
    public class Organizationshopproducts
    {
        public long Shopproductid { get; set; }
        public long Productid { get; set; }
        public string? Productname { get; set; }
        public string? Primaryimageurl { get; set; }
        public long Organizationid { get; set; }
        public decimal Retailprice { get; set; }
        public decimal Wholesaleprice { get; set; }
        public decimal Marketprice { get; set; }
        public string? Productdescription { get; set; }
        public string? ProductSize { get; set; }
        public string? ProductColor { get; set; }
        public string? ProductModel { get; set; }
        public string? ProductStatus { get; set; }
        public decimal ProductStock { get; set; }
        public bool Islipalater { get; set; }
        public decimal Productdepositamount { get; set; }
        public decimal Productinterestrate { get; set; }
        public decimal Periodicamount { get; set; }
        public int PaymentTerm { get; set; }
        public DateTime DateCreated { get; set; }
        public List<ShopProductFeature>? Shopproductfeature { get; set; }
        public List<ShopProductWhatsInBox>? Shopproductwhatsinbox { get; set; }
        public List<ShopProductImage>? ShopProductImage { get; set; }
    }
}
