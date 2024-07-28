using DBL.Entities;
using Mainapp.Services;
namespace Mainapp.Miniapps.Ecommerce.Models
{
    public class Systemorganizationshopproducts : BaseResponse
    {
        public List<Organizationshopproductsdata>? Organizationshopproductsdata { get; set; }
    }
    public class Organizationshopproductsdata
    {
        public int RespStatus { get; set; }
        public string? RespMessage { get; set; }
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
        public string? Productavailability { get; set; }
        public string? Producturl { get; set; }
        public string? Mainproductdescription { get; set; }
        public string? Productbarcode { get; set; }
        public string? Categoryname { get; set; }
        public string? Parentcategoryname { get; set; }
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
        public string? ProductStatus { get; set; }
        public string? ProductGender { get; set; }
        public string? ProductAgeGroup { get; set; }
        public string? ProductMaterial { get; set; }
        public decimal ProductStock { get; set; }
        public bool Islipalater { get; set; }
        public decimal Productdepositamount { get; set; }
        public decimal Productinterestrate { get; set; }
        public decimal Periodicamount { get; set; }
        public int PaymentTerm { get; set; }
        public int Organizationstatus { get; set; }
        public string? Productlink { get; set; }
        public DateTime DateCreated { get; set; }
        public List<ShopProductFeature>? Productfeatures { get; set; }
        public List<ShopProductWhatsInBox>? Productwhatsinbox { get; set; }
        public List<ShopProductImage>? Productotherimages { get; set; }
    }
}
