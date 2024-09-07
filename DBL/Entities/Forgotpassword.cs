using System.ComponentModel.DataAnnotations;

namespace DBL.Entities
{
    public class Forgotpassword
    {
        [Required(ErrorMessage = "Email address can't be empty")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string? Emailaddress { get; set; }
        public string? Androidid { get; set; }
        public long Userid { get; set; }
        public string? Passharsh { get; set; }
        [DataType(DataType.Password)]
        public string? Passwords { get; set; }
        [DataType(DataType.Password)]
        public string? Confirmpasswords { get; set; }
    }
}
