namespace DBL.Entities
{
    public class SystemStaff
    {
        public int Userid { get; set; }
        public string? Firstname { get; set; }
        public string? Lastname { get; set; }
        public string? Phonenumber { get; set; }
        public string? Username { get; set; }
        public string? Emailaddress { get; set; }
        public int Roleid { get; set; }
        public string? Passharsh { get; set; }
        public string? Passwords { get; set; }
        public string? Confirmpasswords { get; set; }
        public bool Isactive { get; set; }
        public bool Isdeleted { get; set; }
        public bool Isdefault { get; set; }
        public string? Loginstatus { get; set; }
        public DateTime? Passwordresetdate { get; set; }
        public int? Parentid { get; set; }
        public string? Extra { get; set; }
        public string? Extra1 { get; set; }
        public string? Extra2 { get; set; }
        public string? Extra3 { get; set; }
        public string? Extra4 { get; set; }
        public string? Extra5 { get; set; }
        public int Createdby { get; set; }
        public int Modifiedby { get; set; }
        public DateTime? Lastlogin { get; set; }
        public DateTime Datemodified { get; set; }
        public DateTime Datecreated { get; set; }
    }
}
