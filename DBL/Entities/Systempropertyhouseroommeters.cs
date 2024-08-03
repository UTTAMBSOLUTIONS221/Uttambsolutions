namespace DBL.Entities
{
    public class Systempropertyhouseroommeters
    {
        public bool Hasprevious { get; set; }
        public int Systempropertyhousemeterid { get; set; }
        public int Systempropertyhouseroomid { get; set; }
        public string? Systempropertyhouseroommeternumber { get; set; }
        public decimal Openingmeter { get; set; }
        public decimal Movedmeter { get; set; }
        public decimal Closingmeter { get; set; }
        public decimal Consumedamount { get; set; }
        public int Createdby { get; set; }
        public DateTime Datecreated { get; set; }
        public List<Systempropertyhouseroommeterhistory>? Data { get; set; }
    }
    public class Systempropertyhouseroommeterhistory
    {
        public int Systempropertyhousemeterid { get; set; }
        public int Systempropertyhouseroomid { get; set; }
        public string? Systempropertyhouseroommeternumber { get; set; }
        public decimal Openingmeter { get; set; }
        public decimal Movedmeter { get; set; }
        public decimal Closingmeter { get; set; }
        public int Createdby { get; set; }
        public DateTime Datecreated { get; set; }
    }
}
