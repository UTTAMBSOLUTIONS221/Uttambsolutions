namespace DBL.Models
{

    public class PropertyHouseRoomTenantModel
    {
        public PropertyHouseRoomTenantData? Data { get; set; }
    }
    public class PropertyHouseRoomTenantData
    {
        public int Userid { get; set; }
        public string? Fullname { get; set; }
        public string? Phonenumber { get; set; }
        public string? Emailaddress { get; set; }
        public string? Gender { get; set; }
        public string? Maritalstatus { get; set; }
        public int Loginstatus { get; set; }
        public bool Isvisible { get; set; }
        public int Parentid { get; set; }
        public string? Userprofileimageurl { get; set; }
        public string? Usercurriculumvitae { get; set; }
        public long Idnumber { get; set; }
        public bool Updateprofile { get; set; }
        public int Accountnumber { get; set; }
        public int Accountid { get; set; }
        public decimal Walletbalance { get; set; }
        public DateTime Datemodified { get; set; }
        public DateTime Datecreated { get; set; }
        public Systempropertyhousetenantsroom? Tenantroomdata { get; set; }
        public List<PropertyHousetenantroomhistory>? Tenantroomhistory { get; set; }
    }
    public class Systempropertyhousetenantsroom
    {
        public int Systempropertyhousetenantentryid { get; set; }
        public int Systempropertyhouseroomid { get; set; }
        public string? Propertyhousename { get; set; }
        public string? Houseoccupationstatus { get; set; }
        public string? Occupationalstatus { get; set; }
        public decimal Systempropertyhousesizerentdeposit { get; set; }
        public decimal Systempropertyhousesizerent { get; set; }
        public decimal Monthlybinfee { get; set; }
        public int Vacatingperioddays { get; set; }
        public decimal Waterunitprice { get; set; }
        public int Previousmeters { get; set; }
        public decimal Previousmeteramount { get; set; }
        public string? Systempropertyhousesizename { get; set; }
        public DateTime Datecreated { get; set; }
        public DateTime Datemodified { get; set; }
        public DateTime? Expectedvacatingdate { get; set; }
        public DateTime? Plannedvacatingdate { get; set; }
        public string? Systempropertyhousevacatingreason { get; set; }
        public List<PropertyHousetenantroommeter>? Tenantroommeter { get; set; }
        public List<PropertyHousetenantroompayments>? Tenantroompayments { get; set; }
    }
    public class PropertyHousetenantroomhistory
    {
        public string? Countyname { get; set; }
        public string? Subcountyname { get; set; }
        public string? Subcountywardname { get; set; }
        public string? Streetorlandmark { get; set; }
        public string? Propertyhousename { get; set; }
        public string? Propertyownername { get; set; }
        public string? Systempropertyhousesizename { get; set; }
        public decimal Outstandingbalance { get; set; }
        public string? Createdby { get; set; }
        public string? Modifiedby { get; set; }
        public DateTime Datemodified { get; set; }
    }
    public class PropertyHousetenantroommeter
    {
        public string? Systempropertyhouseroommeternumber { get; set; }
        public decimal Openingmeter { get; set; }
        public decimal Movedmeter { get; set; }
        public decimal Closingmeter { get; set; }
        public decimal Consumedamount { get; set; }
        public string? Createdby { get; set; }
        public DateTime Datecreated { get; set; }
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
