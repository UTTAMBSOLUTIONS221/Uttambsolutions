using System.ComponentModel.DataAnnotations;

namespace DBL.Entities
{
    public class SystemStaff
    {
        public int Userid { get; set; }
        public string? Firstname { get; set; }
        public string? Lastname { get; set; }
        public string? Fullname { get; set; }
        [DataType(DataType.PhoneNumber)]
        public string? Phonenumber { get; set; }
        public string? Designation { get; set; }
        public string? Username { get; set; }
        [DataType(DataType.EmailAddress)]
        public string? Emailaddress { get; set; }
        public int Genderid { get; set; }
        public int Maritalstatusid { get; set; }
        public int Roleid { get; set; }
        public string? Rolename { get; set; }
        public string? Passharsh { get; set; }
        [DataType(DataType.Password)]
        public string? Passwords { get; set; }
        [DataType(DataType.Password)]
        public string? Confirmpasswords { get; set; }
        public bool Isactive { get; set; }
        public bool Isdeleted { get; set; }
        public bool Isdefault { get; set; }
        public int Loginstatus { get; set; }
        public DateTime? Passwordresetdate { get; set; }
        public int? Parentid { get; set; }
        public string? Userprofileimageurl { get; set; }
        public string? Usercurriculumvitae { get; set; }
        public string? Idnumber { get; set; }
        public bool Updateprofile { get; set; }
        public string? Extra { get; set; }
        public string? Extra1 { get; set; }
        public string? Extra2 { get; set; }
        public string? Extra3 { get; set; }
        public string? Extra4 { get; set; }
        public string? Extra5 { get; set; }
        public int Createdby { get; set; }
        public int Modifiedby { get; set; }
        public int Mpesapaybill { get; set; }
        public int Accountnumber { get; set; }
        public decimal Subscriptionamount { get; set; }
        public string? Kinname { get; set; }
        public string? Kinphonenumber { get; set; }
        public int Kinrelationshipid { get; set; }
        public bool Columnreadonly { get; set; }
        public DateTime? Lastlogin { get; set; }
        public DateTime Datemodified { get; set; }
        public DateTime Datecreated { get; set; }
    }
}
