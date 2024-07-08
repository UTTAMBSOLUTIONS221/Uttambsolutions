namespace DBL.Entities
{
    public class EmailLogs
    {
        public long EmailLogId { get; set; }
        public long TenantId { get; set; }
        public string? EmailAddress { get; set; }
        public string? EmailSubject { get; set; }
        public string? EmailMessage { get; set; }
        public bool IsEmailSent { get; set; }
        public DateTime DateTimeSent { get; set; }
        public DateTime Datecreated { get; set; }
    }
}
