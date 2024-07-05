using DBL.Entities;

namespace DBL.Models
{
    public class SystemOrganizationDetails
    {
        public long OrganizationId { get; set; }
        public long OrganizationOwner { get; set; }
        public string? OrganizationName { get; set; }
        public string? OrganizationDescription { get; set; }
        public int OrganizationTypeId { get; set; }
        public int OrganizationStatus { get; set; }
        public DateTime DateCreated { get; set; }
        public List<Systemproducts>? Systemproducts { get; set; }
    }
}
