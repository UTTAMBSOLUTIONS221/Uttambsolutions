using DBL.Entities;

namespace DBL.Models
{
    public class SystemUserProfileData
    {
        public string Systemmodulename { get; set; }
        public Systemjobdata Systemjobsdata { get; set; }
        public Systemblogdata Systemblogdata { get; set; }
        public Systemorganizationshopproducts Shopproductsdata { get; set; }
        public List<SystemOrganization>? Systemorganizations { get; set; }
        public List<SocialMediaSettings>? Systemusersocials { get; set; }
        public List<Systemblog>? Systemuserblogs { get; set; }

        public List<SystemUserLog> Systemuserlogs { get; set; }
        public List<Systemjobapplications> Systemjobapplications { get; set; }
    }
}
