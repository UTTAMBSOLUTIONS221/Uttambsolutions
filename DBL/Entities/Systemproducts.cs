﻿namespace DBL.Entities
{
    public class Systemproducts
    {
        public long Productid { get; set; }
        public string? Productbarcode { get; set; }
        public string? Productname { get; set; }
        public string? Productdescription { get; set; }
        public int Categoryid { get; set; }
        public string? Categoryname { get; set; }
        public int Brandid { get; set; }
        public string? Brandname { get; set; }
        public string? Sku { get; set; }
        public decimal Wholesaleprice { get; set; }
        public decimal Retailprice { get; set; }
        public string? Primaryimageurl { get; set; }
    }

}
