using System;
using System.Reflection.Metadata;

namespace DBL.Models
{
    public class Newsapiarticles
    {
        public string? SourceId { get; set; }
        public string? SourceName { get; set; }
        public string? Author { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? Url { get; set; }
        public string? UrlToImage { get; set; }
        public DateTime? PublishedAt { get; set; }
        public string? Content { get; set; }
    }
}
