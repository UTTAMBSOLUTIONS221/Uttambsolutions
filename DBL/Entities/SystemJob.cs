namespace DBL.Entities
{
    public class SystemJob
    {
        public int JobId { get; set; }
        public int EmployerId { get; set; }
        public string Title { get; set; }
        public string JobDescription { get; set; }
        public int? JobFunctionId { get; set; }
        public int? JobIndustryId { get; set; }
        public int? JobLocationId { get; set; }
        public int? JobTypeId { get; set; }
        public string JobTypeName { get; set; }
        public int? JobExperienceId { get; set; }
        public string JobSalaryRange { get; set; }
        public DateTime? Deadline { get; set; }
        public string JobStatus { get; set; } = "Open";
        public DateTime DatePosted { get; set; } = DateTime.Now;
        public string JobUrl { get; set; }
        public bool EasyApply { get; set; } = false;
        public bool HasTest { get; set; } = false;
        public bool Approved { get; set; } = false;
        public bool IsPublished { get; set; } = false;
        public DateTime DateCreated { get; set; } = DateTime.Now;
    }
}
