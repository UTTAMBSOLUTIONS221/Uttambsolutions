using DBL.Entities;

namespace DBL.Models
{
    public class Systemorganizationshopproducts
    {
        public long Shopproductid { get; set; }
        public long Productid { get; set; }
        public long Organizationid { get; set; }
        public string? Fullname { get; set; }
        public string? Organizationname { get; set; }
        public string? OrganizationEmail { get; set; }
        public string? OrganizationPhone { get; set; }
        public string? Organizationdescription { get; set; }
        public string? Productname { get; set; }
        public string? Productdescription { get; set; }
        public string? Primaryimageurl { get; set; }
        public string? Mainproductdescription { get; set; }
        public string? Productbarcode { get; set; }
        public string? Categoryname { get; set; }
        public string? Brandname { get; set; }
        public string? Sku { get; set; }
        public int Brandid { get; set; }
        public int Categoryid { get; set; }
        public decimal Wholesaleprice { get; set; }
        public decimal Retailprice { get; set; }
        public decimal Vatrate { get; set; }
        public string? Imageurl { get; set; }
        public string? ProductSize { get; set; }
        public string? ProductColor { get; set; }
        public string? ProductModel { get; set; }
        public decimal Marketprice { get; set; }
        public decimal ProductStock { get; set; }
        public int Organizationstatus { get; set; }
        public List<ShopProductFeature>? Productfeatures { get; set; }
        public List<ShopProductWhatsInBox>? Productwhatsinbox { get; set; }
        public List<ShopProductImage>? Productotherimages { get; set; }
    }
}
