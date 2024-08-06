namespace DBL.Models
{
    public class PropertyHouseRoomTenantData
    {
        public int Userid { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Phonenumber { get; set; }
        public string Username { get; set; }
        public string Emailaddress { get; set; }
        public int Genderid { get; set; }
        public string Gender { get; set; }
        public int Maritalstatusid { get; set; }
        public string Maritalstatus { get; set; }
        public bool Isactive { get; set; }
        public bool Isdeleted { get; set; }
        public bool Isdefault { get; set; }
        public bool Loginstatus { get; set; }
        public DateTime? Passwordresetdate { get; set; }
        public int? Parentid { get; set; }
        public string Userprofileimageurl { get; set; }
        public string Usercurriculumvitae { get; set; }
        public string Idnumber { get; set; }
        public bool Updateprofile { get; set; }
        public string Accountnumber { get; set; }
        public int Accountid { get; set; }
        public decimal Walletbalance { get; set; }
        public string Createdby { get; set; }
        public string Modifiedby { get; set; }
        public DateTime? Lastlogin { get; set; }
        public DateTime? Datemodified { get; set; }
        public DateTime? Datecreated { get; set; }
        public Systempropertyhousetenantsroom Data { get; set; }
    }
    public class Systempropertyhousetenantsroom
    {
        public int Systempropertyhousetenantentryid { get; set; }
        public int Systempropertyhouseroomid { get; set; }
        public string Propertyhousename { get; set; }
        public bool Isoccupant { get; set; }
        public string Houseoccupationstatus { get; set; }
        public decimal Systempropertyhousesizerentdeposit { get; set; }
        public decimal Systempropertyhousesizerent { get; set; }
        public decimal Monthlybinfee { get; set; }
        public decimal Waterunitprice { get; set; }
        public decimal Previousmeters { get; set; }
        public decimal Previousmeteramount { get; set; }
        public string Systempropertyhousesizename { get; set; }
        public DateTime? Datecreated { get; set; }
        public DateTime? Datemodified { get; set; }

        public List<PropertyHousetenantroommeter> Tenantroommeter { get; set; }
        public List<PropertyHousetenantroomhistory> Tenantroomhistory { get; set; }
        public List<PropertyHousetenantroompayments> Tenantroompayments { get; set; }
    }
    public class PropertyHousetenantroomhistory
    {
        public int Houseid { get; set; }
        public string Propertyhousename { get; set; }
        public string Systempropertyhousesizename { get; set; }
        public DateTime? Datecreated { get; set; }
        public DateTime? Datemodified { get; set; }
    }
    public class PropertyHousetenantroommeter
    {
        public int Houseid { get; set; }
        public decimal Openingmeter { get; set; }
        public decimal Closingmeter { get; set; }
        public decimal Units { get; set; }
        public decimal Billedamount { get; set; }
        public DateTime? Datecreated { get; set; }
        public DateTime? Datemodified { get; set; }
    }
    public class PropertyHousetenantroompayments
    {
        public int Houseid { get; set; }
        public decimal Monthlyrentamount { get; set; }
        public decimal Paymentamount { get; set; }
        public DateTime? Datecreated { get; set; }
        public DateTime? Datemodified { get; set; }
    }
}
