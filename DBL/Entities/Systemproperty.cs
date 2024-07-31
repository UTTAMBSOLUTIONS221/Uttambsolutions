﻿namespace DBL.Entities
{
    public class Systemproperty
    {
        public long Propertyhouseid { get; set; }
        public bool Isagency { get; set; }
        public long Propertyhouseowner { get; set; }
        public long Propertyhouseposter { get; set; }
        public string? Propertyhousename { get; set; }
        public int Countyid { get; set; }
        public int Subcountyid { get; set; }
        public int Subcountywardid { get; set; }
        public string? Streetorlandmark { get; set; }
        public bool Hashousedeposit { get; set; }
        public int Propertyhousestatus { get; set; }
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
        public List<Systemhousedepositfees>? Housedepositfees { get; set; }
    }
    public class Systempropertyhousesize
    {
        public long Systempropertyhousesizeid { get; set; }
        public long Propertyhouseid { get; set; }
        public int Systempropertyhousesizeunits { get; set; }
        public decimal Systempropertyhousesizerent { get; set; }
        public bool Systempropertyhousesizedeposit { get; set; }
    }

    public class Systemhousebenefits
    {
        public long Housebenefitid { get; set; }
        public string? Housebenefitname { get; set; }
    }

    public class Systemhousedepositfees
    {
        public long Propertyid { get; set; }
        public long Housedepositfeeid { get; set; }
        public decimal Housedepositamount { get; set; }
    }
}
