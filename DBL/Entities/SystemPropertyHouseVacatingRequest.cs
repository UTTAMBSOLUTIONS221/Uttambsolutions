namespace DBL.Entities
{
    public class SystemPropertyHouseVacatingRequest
    {
        public long VacatingRequestId { get; set; }
        public long SystemPropertyHouseTenantId { get; set; }
        public long SystemPropertyHouseRoomId { get; set; }
        public DateTime? PlannedVacatingDate { get; set; }
        public DateTime? ExpectedVacatingDate { get; set; }
        public string? VacatingReason { get; set; }
        public int VacatingStatus { get; set; }
        public long ApprovedBy { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateApproved { get; set; }
    }
}
