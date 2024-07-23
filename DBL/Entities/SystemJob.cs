﻿namespace DBL.Entities
{
    public class SystemJob
    {
        public int JobId { get; set; }
        public long EmployerId { get; set; }
        public string? Employername { get; set; }
        public string? Employerlogo { get; set; }
        public string? Title { get; set; }
        public string? JobDescription { get; set; }
        public int? JobFunctionId { get; set; }
        public string? Functionname { get; set; }
        public int? JobIndustryId { get; set; }
        public string? Jobindustryname { get; set; }
        public int? JobLocationId { get; set; }
        public string? Locationname { get; set; }
        public int? JobTypeId { get; set; }
        public string? Jobtypename { get; set; }
        public int? JobExperienceId { get; set; }
        public string? Experiencename { get; set; }
        public string? JobSalaryRange { get; set; }
        public DateTime Deadline { get; set; } = DateTime.Now;
        public string? JobStatus { get; set; } = "Open";
        public DateTime DatePosted { get; set; } = DateTime.Now;
        public string? JobUrl { get; set; }
        public string? JobPostUrl { get; set; }
        public bool EasyApply { get; set; } = false;
        public bool HasTest { get; set; } = false;
        public bool IsFeatured { get; set; } = false;
        public bool Approved { get; set; } = false;
        public bool IsPublished { get; set; } = false;
        public long CreatedBy { get; set; }

        public string? Jobreportto { get; set; }
        public string? Jobimageburl { get; set; }
        public string? jobhowtoapply { get; set; }
        public DateTime DateCreated { get; set; } = DateTime.Now;
        public List<SystemJobKeyResponsibility> SystemJobKeyResponsibility { get; set; }
    }

    public class SystemJobKeyResponsibility
    {
        public int JobKeyResponsibilityId { get; set; }
        public int JobId { get; set; }
        public string? Responsibility { get; set; }
        public DateTime DateCreated { get; set; }
        public List<SystemJobKeyResponsibilityFunction> SystemJobKeyResponsibilityFunction { get; set; }
    }

    public class SystemJobKeyResponsibilityFunction
    {
        public int JobKeyResponsibilityFunctionId { get; set; }
        public int JobKeyResponsibilityId { get; set; }
        public string? ResponsibilityFunction { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
