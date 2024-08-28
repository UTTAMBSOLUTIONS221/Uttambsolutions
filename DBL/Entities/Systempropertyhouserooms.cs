using DBL.Models;

namespace DBL.Entities
{
    public class Systempropertyhouserooms
    {
        public bool Hasprevious { get; set; }
        public long Systempropertyhouseroomid { get; set; }
        public long Systempropertyhouseid { get; set; }
        public long Systempropertyhousesizeid { get; set; }
        public string? Systempropertyhousesizename { get; set; }
        public decimal Systempropertyhousesizerent { get; set; }
        public bool Systempropertyhousesizedeposit { get; set; }
        public bool Isvacant { get; set; }
        public bool Isunderrenovation { get; set; }
        public bool Isshop { get; set; }
        public bool Isgroundfloor { get; set; }
        public bool Hasbalcony { get; set; }
        public bool Forcaretaker { get; set; }
        public long Kitchentypeid { get; set; }
        public int Systempropertyhousemeterid { get; set; }
        public string? Systempropertyhouseroommeternumber { get; set; }
        public decimal Openingmeter { get; set; }
        public decimal Movedmeter { get; set; }
        public decimal Closingmeter { get; set; }
        public int Roomoccupant { get; set; }
        public string? Roomoccupantdetail { get; set; }
        public long Systempropertyhousetenantid { get; set; }
        public decimal Consumedamount { get; set; }
        public bool Hashousewatermeter { get; set; }
        public decimal Waterunitprice { get; set; }
        public long Tenantid { get; set; }
        public int Createdby { get; set; }
        public DateTime Datecreated { get; set; }
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
        public List<Systempropertyhouseroommeterhistory>? Meterhistorydata { get; set; }
        public List<PropertyHousetenantroomhistory>? Roomtenanthistorydata { get; set; }
    }

}
