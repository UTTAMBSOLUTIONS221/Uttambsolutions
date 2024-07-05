using System.ComponentModel.DataAnnotations;

namespace DBL.Entities
{
    public class SystemStaff
    {
        public int Userid { get; set; }
        [Required(ErrorMessage = "Firstname is Required!")]
        public string? Firstname { get; set; }
        [Required(ErrorMessage = "Lastname is Required!")]
        public string? Lastname { get; set; }
        [Required(ErrorMessage = "Phonenumber is Required!")]
        [DataType(DataType.PhoneNumber)]
        public string? Phonenumber { get; set; }
        public string? Username { get; set; }
        [Required(ErrorMessage = "Emailaddress is Required!")]
        [DataType(DataType.EmailAddress)]
        public string? Emailaddress { get; set; }
        public int Roleid { get; set; }
        public string? Rolename { get; set; }

        public string? Passharsh { get; set; }

        [Required(ErrorMessage = "Password is Required!")]
        public string? Passwords { get; set; }
        [Required(ErrorMessage = "Confirm Password is required.")]
        [DataType(DataType.Password)]
        [Compare("Passwords", ErrorMessage = "The password and confirmation password do not match.")]
        public string? Confirmpasswords { get; set; }
        public bool Isactive { get; set; }
        public bool Isdeleted { get; set; }
        public bool Isdefault { get; set; }
        public int Loginstatus { get; set; }
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
