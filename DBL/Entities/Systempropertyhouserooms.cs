namespace DBL.Entities
{
    public class Systempropertyhouserooms
    {
        public bool Hasprevious { get; set; }
        public long Systempropertyhouseroomid { get; set; }
        public long Systempropertyhouseid { get; set; }
        public long Systempropertyhousesizeid { get; set; }
        public string? Systempropertyhousesizename { get; set; }
        public decimal Systempropertyhousesizerent { get; set; }
        public bool Systempropertyhousesizedeposit { get; set; }
        public bool Isvacant { get; set; }
        public bool Isunderrenovation { get; set; }
        public bool Isshop { get; set; }
        public bool Isgroundfloor { get; set; }
        public bool Hasbalcony { get; set; }
        public bool Forcaretaker { get; set; }
        public long Kitchentypeid { get; set; }
        public int Systempropertyhousemeterid { get; set; }
        public string? Systempropertyhouseroommeternumber { get; set; }
        public decimal Openingmeter { get; set; }
        public decimal Movedmeter { get; set; }
        public decimal Closingmeter { get; set; }
        public decimal Consumedamount { get; set; }
        public bool Hashousewatermeter { get; set; }
        public decimal Waterunitprice { get; set; }
        public long Tenantid { get; set; }
        public int Createdby { get; set; }
        public DateTime Datecreated { get; set; }
        public List<Systempropertyhouseroommeterhistory>? Meterhistorydata { get; set; }
    }

}
