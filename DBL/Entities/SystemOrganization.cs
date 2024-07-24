namespace DBL.Entities
{
    public class SystemOrganization
    {
        public long OrganizationId { get; set; }
        public long OrganizationOwner { get; set; }
        public string? OrganizationName { get; set; }
        public string? OrganizationDescription { get; set; }
        public string? OrganizationLogo { get; set; }
        public int OrganizationTypeId { get; set; }
        public int OrganizationStatus { get; set; }
        public string? OrganizationPhone { get; set; }
        public string? OrganizationEmail { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
