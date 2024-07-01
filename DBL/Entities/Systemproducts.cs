namespace DBL.Entities
{
    public class Systemproducts
    {
        public long Productid { get; set; }
        public long Shopid { get; set; }
        public string? Productcode { get; set; }
        public string? Productbarcode { get; set; }
        public string? Productname { get; set; }
        public string? Productdescription { get; set; }
        public int Categoryid { get; set; }
        public string? Status { get; set; }
        public string? Sku { get; set; }
        public decimal Price { get; set; }
        public decimal Originalprice { get; set; }
        public string? Size { get; set; }
        public string? Color { get; set; }
        public string? Model { get; set; }
        public decimal Stock { get; set; }
        public string? Primaryimageurl { get; set; }
    }

}
