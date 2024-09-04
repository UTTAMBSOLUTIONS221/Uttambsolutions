namespace DBL.Models
{
    public class TenantAgreementDetailDataModel
    {
        public TenantAgreementDetailData? Data { get; set; }
    }
    public class TenantAgreementDetailData
    {
        public long Userid { get; set; }
        public long Propertyhouseid { get; set; }
        public string? Tenantfullname { get; set; }
        public string? Tenantphonenumber { get; set; }
        public string? Tenantemailaddress { get; set; }
        public int Tenantidnumber { get; set; }
        public string? Ownerfullname { get; set; }
        public string? Ownerphonenumber { get; set; }
        public string? Owneremailaddress { get; set; }
        public int Owneridnumber { get; set; }
        public string? Propertyhousename { get; set; }
        public int Rentdueday { get; set; }
        public int Vacantnoticeperiod { get; set; }
        public bool Hasagent { get; set; }
        public bool Hashousewatermeter { get; set; }
        public string? Systemhousewatertypename { get; set; }
        public string? Systempropertyhousesizename { get; set; }
        public decimal Systempropertyhousesizerent { get; set; }
        public decimal Systempropertyhousesizerentdeposit { get; set; }
        public int Rentdepositmonth { get; set; }
        public int Rentdepositrefunddays { get; set; }
        public bool Monthlyrentterms { get; set; }
        public bool Allowpets { get; set; }
        public bool Rentutilityinclusive { get; set; }
        public decimal Waterunitprice { get; set; }
        public string? Countyname { get; set; }
        public string? Subcountyname { get; set; }
        public string? Subcountywardname { get; set; }
        public string? Streetorlandmark { get; set; }
        public DateTime TenantDatecreated { get; set; }
        public DateTime Nextrentduedate { get; set; }
        public string? TenantSignatureimageurl { get; set; }
        public string? OwnerSignatureimageurl { get; set; }
        public string? Agreementdata { get; set; }
        public string? Propertyhouseutility { get; set; }
        public string? Systempropertybankname { get; set; }
        public string? Tenantsintheroom { get; set; }
        public string? Termenddate { get; set; }



        public long Agreementid { get; set; }
        public long Propertyhouseowner { get; set; }
        public string? Signatureimageurl { get; set; }
        public string? Agreementname { get; set; }
        public DateTime Datecreated { get; set; }
        public string? Ownerortenant { get; set; }
        public string? Agreementdetailpdfurl { get; set; }
    }

}
