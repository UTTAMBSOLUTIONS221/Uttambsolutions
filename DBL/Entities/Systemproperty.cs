namespace DBL.Entities
{
    public class Systemproperty
    {
        public long Propertyhouseid { get; set; }
        public bool Isagency { get; set; }
        public int Roomscount { get; set; }
        public long Propertyhouseowner { get; set; }
        public string? Primaryimageurl { get; set; }
        public string? Propertyhouseownername { get; set; }
        public long Propertyhouseposter { get; set; }
        public string? Propertyhousename { get; set; }
        public int Countyid { get; set; }
        public string? Countyname { get; set; }
        public int Subcountyid { get; set; }
        public string? Subcountyname { get; set; }
        public int Subcountywardid { get; set; }
        public string? Subcountywardname { get; set; }
        public string? Streetorlandmark { get; set; }
        public string? Contactdetails { get; set; }
        public string? Rentingterms { get; set; }
        public DateTime Enddate { get; set; } = DateTime.UtcNow;
        public bool Hashousedeposit { get; set; }
        public bool Hasagent { get; set; }
        public bool Allowpets { get; set; }
        public int Numberofpets { get; set; }
        public decimal Petdeposit { get; set; }
        public string? Petparticulars { get; set; }
        public bool Rentutilityinclusive { get; set; }
        public bool Hashousewatermeter { get; set; }
        public int Propertyhousestatus { get; set; }
        public int Watertypeid { get; set; }
        public decimal Waterunitprice { get; set; }
        public int Rentdueday { get; set; }
        public int Rentdepositmonth { get; set; }
        public int Rentdepositreturndays { get; set; }
        public int Vacantnoticeperiod { get; set; }
        public string? Propertyhousestatusdata { get; set; }
        public decimal Monthlycollection { get; set; }
        public string? Extra { get; set; }
        public string? Extra1 { get; set; }
        public string? Extra2 { get; set; }
        public string? Extra3 { get; set; }
        public string? Extra4 { get; set; }
        public string? Extra5 { get; set; }
        public string? Extra6 { get; set; }
        public string? Extra7 { get; set; }
        public string? Extra8 { get; set; }
        public string? Extra9 { get; set; }
        public string? Extra10 { get; set; }
        public long Createdby { get; set; }
        public long Modifiedby { get; set; }
        public DateTime Datecreated { get; set; }
        public DateTime Datemodified { get; set; }
        public List<Systempropertyhousesize>? Propertyhousesize { get; set; }
        public List<Systempropertyhousedepositfees>? Propertyhousedepositfee { get; set; }
        public List<Propertyhousebankingdetail>? Propertyhousebankingdetail { get; set; }
        public List<Systempropertyhousebenefits>? Propertyhousebenefit { get; set; }
    }
    public class Systempropertyhousesize
    {
        public long Systempropertyhousesizeid { get; set; }
        public long Propertyhouseid { get; set; }
        public int Systempropertyhousesizeunits { get; set; }
        public long? Systemhousesizeid { get; set; }
        public string? Systemhousesizename { get; set; }
        public bool Systempropertyhousesizewehave { get; set; }
    }

    public class Propertyhousebankingdetail
    {
        public int Systempropertybankaccountid { get; set; }
        public int Propertyhouseid { get; set; }

        public int Systembankid { get; set; }
        public string? Systembanknameandpaybill { get; set; }

        public string? Systempropertybankaccount { get; set; }

        public bool Systempropertyhousebankwehave { get; set; }
    }

    public class Systempropertyhousedepositfees
    {
        public long Systempropertyhousedepositfeeid { get; set; }
        public long Propertyhouseid { get; set; }
        public long Housedepositfeeid { get; set; }
        public string? Housedepositfeename { get; set; }
        public decimal Systempropertyhousedepositfeeamount { get; set; }
        public bool Systempropertyhousesizedepositfeewehave { get; set; }
    }

    public class Systempropertyhousebenefits
    {
        public long Systempropertyhousebenefitid { get; set; }
        public long Propertyhouseid { get; set; }
        public long Housebenefitid { get; set; }
        public string? Housebenefitname { get; set; }
        public bool Systempropertyhousebenefitwehave { get; set; }
    }
}
