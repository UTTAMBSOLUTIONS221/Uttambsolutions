namespace DBL.Entities
{
    public class Productcategories
    {
        public int Categoryid { get; set; }
        public string? Categoryname { get; set; }
        public string? Parentcategoryname { get; set; }
        public decimal Vatrate { get; set; }
        public string? Imageurl { get; set; }
    }
}
