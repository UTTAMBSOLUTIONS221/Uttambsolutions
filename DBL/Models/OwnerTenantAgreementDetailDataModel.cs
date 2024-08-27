namespace DBL.Models
{
    public class OwnerTenantAgreementDetailDataModel
    {
        public OwnerTenantAgreementDetailData? Data { get; set; }
    }
    public class OwnerTenantAgreementDetailData
    {
        public long Agreementid { get; set; }
        public long Propertyhouseid { get; set; }
        public long Propertyhouseowner { get; set; }
        public string? Propertyhousename { get; set; }
        public string? Fullname { get; set; }
        public string? Phonenumber { get; set; }
        public string? Emailaddress { get; set; }
        public string? Countyname { get; set; }
        public string? Subcountyname { get; set; }
        public string? Subcountywardname { get; set; }
        public DateTime OwnerDatecreated { get; set; }
        public DateTime Datecreated { get; set; }
        public string? OwnerSignatureimageurl { get; set; }
        public string? Agreementname { get; set; }
        public string? Ownerortenant { get; set; }
        public string? Signatureimageurl { get; set; }
        public string? Agreementdetailpdfurl { get; set; }
    }
}
