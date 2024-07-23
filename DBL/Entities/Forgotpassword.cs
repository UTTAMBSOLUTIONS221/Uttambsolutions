using System.ComponentModel.DataAnnotations;

namespace DBL.Entities
{
    public class Forgotpassword
    {
        [Required(ErrorMessage = "Email address can't be empty")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string? Emailaddress { get; set; }
    }
}
