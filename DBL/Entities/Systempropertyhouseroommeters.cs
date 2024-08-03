namespace DBL.Entities
{
    public class Systempropertyhouseroommeters
    {
        public int Systempropertyhousemeterid { get; set; }
        public int Systempropertyhouseroomid { get; set; }
        public string? Systempropertyhouseroommeternumber { get; set; }
        public decimal Openinmeter { get; set; }
        public decimal Movedmeter { get; set; }
        public decimal Closingmeter { get; set; }
        public int Createdby { get; set; }
        public DateTime Datecreated { get; set; }
    }

}
