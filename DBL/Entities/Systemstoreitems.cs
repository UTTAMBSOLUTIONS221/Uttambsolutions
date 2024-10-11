namespace DBL.Entities
{
    public class Systemstoreitems
    {
        public int Storeitemid { get; set; }
        public string? Storeitemname { get; set; }
        public string? Itembrandname { get; set; }
        public string? Itemsize { get; set; }
        public decimal Itembuyingprice { get; set; }
        public decimal Itemsellingprice { get; set; }
        public int Itemstatus { get; set; }
        public bool Isactive { get; set; }
        public bool Isdeleted { get; set; }
        public int Createdby { get; set; }
        public int Modifiedby { get; set; }
        public DateTime Datecreated { get; set; }
        public DateTime Datemodified { get; set; }
        public string? Storeproductimgurl { get; set; }
        public string? Productdescription { get; set; }
        public string? Productavailability { get; set; }
        public string? Productstatus { get; set; }
        public string? Parentcategoryname { get; set; }
        public decimal Productstock { get; set; }
        public string? Productgender { get; set; }
        public string? Productcolor { get; set; }
        public string? Productagegroup { get; set; }
        public string? Productmaterial { get; set; }
        public List<Storeproductimages>? Storeproductimages { get; set; }
    }
    public class Storeproductimages
    {
        public int Storeitemid { get; set; }
        public string? Storeproductimgurl { get; set; }
        public DateTime Datecreated { get; set; }
    }
}
