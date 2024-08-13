namespace DBL.Models
{
    public class Systemstaffdetaildata
    {
        public StaffDetailData? Data { get; set; }
    }

    public class StaffDetailData
    {
        public int Userid { get; set; }
        public string? Fullname { get; set; }
        public string? Phonenumber { get; set; }
        public int Loginstatus { get; set; }
        public int Accountid { get; set; }
        public int Accountnumber { get; set; }
        public decimal Monthlysubscriptionfee { get; set; }
        public List<AccountVerificationBank>? AccountVerificationBanks { get; set; }
    }
    public class AccountVerificationBank
    {
        public int Verificationid { get; set; }
        public string? Verificationname { get; set; }
        public string? Verificationtype { get; set; }
        public int Verificationshortcode { get; set; }
        public string? Accountnumber { get; set; }
        public bool Isactive { get; set; }
    }

}
