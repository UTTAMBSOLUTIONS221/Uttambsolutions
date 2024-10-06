namespace DBL.Models
{
    public class Maqaoplussummary
    {
        public int Listedproperties { get; set; } = 0;
        public int Listedjobs { get; set; } = 0;
        public int Registeredtenants { get; set; } = 0;
        public int Occupiedhouses { get; set; } = 0;
        public decimal Collectedrent { get; set; } = 0;
        public List<Vacanthousesdata>? Vacanthouses { get; set; }
    }
    public class Vacanthousesdata
    {
        public int SystempropertyhouseroomId { get; set; }
        public int Systempropertyhouseid { get; set; }
        public string? Propertyhousestatusdata { get; set; }
        public string? Primaryimageurl { get; set; }
        public string? Propertyhousename { get; set; }
        public string? Countyname { get; set; }
        public string? Subcountyname { get; set; }
        public string? Subcountywardname { get; set; }
        public string? Streetorlandmark { get; set; }
        public bool Hashousedeposit { get; set; }
        public bool Hashousewatermeter { get; set; }
        public bool Allowpets { get; set; }
        public int Rentdepositmonth { get; set; }
        public bool Hasagent { get; set; }
        public int Rentdueday { get; set; }
        public int Rentdepositreturndays { get; set; }
        public string? Rentingterms { get; set; }
        public bool Rentutilityinclusive { get; set; }
        public string? SystemHousesizename { get; set; }
        public int Systempropertyhousesizeid { get; set; }
        public string? Systempropertyhousesizename { get; set; }
        public decimal Systempropertyhousesizerent { get; set; }
        public decimal Systempropertyhousesizedeposit { get; set; }
        public bool Isvacant { get; set; }
        public bool Isunderrenovation { get; set; }
        public bool Isshop { get; set; }
        public bool Isgroundfloor { get; set; }
        public bool Hasbalcony { get; set; }
        public bool Forcaretaker { get; set; }
        public int Kitchentypeid { get; set; }
        public string? Kitchentypename { get; set; }
    }
}
