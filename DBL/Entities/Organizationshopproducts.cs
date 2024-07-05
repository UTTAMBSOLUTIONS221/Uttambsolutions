namespace DBL.Entities
{
    public class Organizationshopproducts
    {
        public long Shopproductid { get; set; }
        public long Productid { get; set; }
        public string? Productname { get; set; }
        public long Organizationid { get; set; }
        public decimal Retailprice { get; set; }
        public decimal Wholesaleprice { get; set; }
        public decimal Marketprice { get; set; }
        public string? Productdescription { get; set; }
        public string? ProductSize { get; set; }
        public string? ProductColor { get; set; }
        public string? ProductModel { get; set; }
        public decimal ProductStock { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
