namespace DBL.Models
{
    public class UsermodeldataResponce
    {
        public int Userid { get; set; }
        public string? Firstname { get; set; }
        public string? Lastname { get; set; }
        public string? Fullname { get; set; }
        public string? Phonenumber { get; set; }
        public string? Username { get; set; }
        public string? Emailaddress { get; set; }
        public int Genderid { get; set; }
        public int Maritalstatusid { get; set; }
        public int Roleid { get; set; }
        public string? Passharsh { get; set; }
        public string? Passwords { get; set; }
        public bool Isactive { get; set; }
        public bool Isdeleted { get; set; }
        public bool Isdefault { get; set; }
        public int Loginstatus { get; set; }
        public string? Designation { get; set; }
        public DateTime? Passwordresetdate { get; set; }
        public int? Parentid { get; set; }
        public string? Userprofileimageurl { get; set; }
        public string? Usercurriculumvitae { get; set; }
        public string? Idnumber { get; set; }
        public bool Updateprofile { get; set; }
        public int Accountnumber { get; set; } = 0;
        public decimal Walletbalance { get; set; } = 0;
        public string? Rolename { get; set; }
        public string? RoleDescription { get; set; }
        public int Tenantid { get; set; }
        public int Createdby { get; set; }
        public int Modifiedby { get; set; }
        public DateTime? Lastlogin { get; set; }
        public DateTime? Datemodified { get; set; }
        public DateTime Datecreated { get; set; }
    }
}
