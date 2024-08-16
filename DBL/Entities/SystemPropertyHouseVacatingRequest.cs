﻿namespace DBL.Entities
{
    public class SystemPropertyHouseVacatingRequest
    {
        public long Vacatingrequestid { get; set; }
        public long Systempropertyhousetenantid { get; set; }
        public long Systempropertyhouseroomid { get; set; }
        public DateTime? Plannedvacatingdate { get; set; }
        public DateTime? Expectedvacatingdate { get; set; }
        public string? Vacatingreason { get; set; }
        public int Vacatingstatus { get; set; }
        public long Approvedby { get; set; }
        public DateTime Datecreated { get; set; }
        public DateTime? Dateapproved { get; set; }
    }
}
