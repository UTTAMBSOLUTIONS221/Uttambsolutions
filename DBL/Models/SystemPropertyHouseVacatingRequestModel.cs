namespace DBL.Models
{
    public class SystemPropertyHouseVacatingRequestModel
    {
        public List<HouseVacatingRequestModel>? Data { get; set; }
    }

    public class HouseVacatingRequestModel
    {
        public int VacatingRequestId { get; set; }

        public int SystemPropertyHouseTenantId { get; set; }

        public string? SystemTenantName { get; set; }

        public string? SystemPropertyHouseSizeName { get; set; }

        public int SystemPropertyHouseRoomId { get; set; }

        public DateTime PlannedVacatingDate { get; set; }

        public DateTime ExpectedVacatingDate { get; set; }

        public string? VacatingReason { get; set; }

        public bool VacatingStatus { get; set; }

        public string? OccupationalStatus { get; set; }

        public int ApprovedBy { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime? DateApproved { get; set; }
    }
}
