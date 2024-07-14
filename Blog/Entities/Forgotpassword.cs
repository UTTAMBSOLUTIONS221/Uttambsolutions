using System.ComponentModel.DataAnnotations;

namespace Blog.Entities
{
    public class Forgotpassword
    {
        [Required(ErrorMessage = "Email address can't be empty")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string? Emailaddress { get; set; }
    }
}
