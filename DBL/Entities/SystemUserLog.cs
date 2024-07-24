namespace DBL.Entities
{
    public class SystemUserLog
    {
        public int LogId { get; set; }
        public int UserId { get; set; }
        public string LogAction { get; set; }
        public string Browser { get; set; }
        public string IpAddress { get; set; }
        public decimal LoyaltyReward { get; set; }
        public int LoyaltyStatus { get; set; }
        public int LogActionExitTime { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
