using DBL.Entities;

namespace DBL.Models
{
    public class SystemUserProfileData
    {
        public long Userid { get; set; }
        public string? Firstname { get; set; }
        public string? Lastname { get; set; }
        public string? UserProfileImageUrl { get; set; }
        public string? Fullname { get; set; }
        public string? Phonenumber { get; set; }
        public string? Username { get; set; }
        public string? Emailaddress { get; set; }
        public int Roleid { get; set; }
        public string? Rolename { get; set; }
        public bool Isactive { get; set; }
        public bool Isdeleted { get; set; }
        public int Loginstatus { get; set; }
        public DateTime Passwordresetdate { get; set; }
        public DateTime Lastlogin { get; set; }
        public DateTime Datemodified { get; set; }
        public DateTime Datecreated { get; set; }
        public int TotalPosts { get; set; }
        public int TotalComments { get; set; }
        public DateTime LastLogin { get; set; }
        public string? Status { get; set; }
        public List<SystemOrganization>? Systemorganizations { get; set; }
        public List<SocialMediaSettings>? Systemusersocials { get; set; }
        public List<Systemblog>? Systemuserblogs { get; set; }
        public List<SystemJob> Systemjobs { get; set; }
        public List<SystemUserLog> Systemuserlogs { get; set; }
    }
}
