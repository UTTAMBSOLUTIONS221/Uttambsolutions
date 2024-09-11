namespace DBL.Models
{
    public class Systemtenantdetailsdata
    {
        public Systemtenantdetails? Data { get; set; }
    }
    public class Systemtenantdetails
    {
        public int Userid { get; set; }
        public string? Firstname { get; set; }
        public string? Lastname { get; set; }
        public string? Fullname { get; set; }
        public string? Phonenumber { get; set; }
        public string? Emailaddress { get; set; }
        public int Loginstatus { get; set; }
        public long Idnumber { get; set; }
        public int Propertyhouseid { get; set; }
        public decimal Walletbalance { get; set; }

        public List<PropertyHouseTenant>? Tenantroomhistory { get; set; }
    }
}
