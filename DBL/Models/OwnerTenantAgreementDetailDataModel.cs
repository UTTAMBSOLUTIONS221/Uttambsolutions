namespace DBL.Models
{
    public class OwnerTenantAgreementDetailDataModel
    {
        public OwnerTenantAgreementDetailData? Data { get; set; }
    }
    public class OwnerTenantAgreementDetailData
    {
        public int Propertyhouseid { get; set; }
        public string? Propertyhouseowner { get; set; }
        public string? Fullname { get; set; }
        public string? Phonenumber { get; set; }
        public string? Emailaddress { get; set; }
        public string? Countyname { get; set; }
        public string? Subcountyname { get; set; }
        public string? Subcountywardname { get; set; }
        public DateTime OwnerDatecreated { get; set; }
        public DateTime TenantDatecreated { get; set; }
        public string? OwnerSignatureimageurl { get; set; }
        public string? TenantSignatureimageurl { get; set; }
    }

}
