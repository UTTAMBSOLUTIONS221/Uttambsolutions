using System.ComponentModel.DataAnnotations;

namespace DBL.Entities
{
    public class Systemblog
    {
        [Key]
        public long Blogid { get; set; }

        [Required]
        public long Blogcategoryid { get; set; }
        public string? Blogcategoryname { get; set; }

        [Required]
        [StringLength(400)]
        public string? Blogname { get; set; }

        [Required]
        [StringLength(4000)]
        public string? Blogcontent { get; set; }

        [Required]
        [StringLength(2000)]
        public string? Summary { get; set; }

        [Required]
        [StringLength(200)]
        public string? Blogprimaryimageurl { get; set; }

        [Required]
        [StringLength(400)]
        public string? Blogtags { get; set; }

        [Required]
        public long Blogowner { get; set; }
        public string? Blogownername { get; set; }

        [Required]
        public bool IsPublished { get; set; }

        [Required]
        public int Blogstatus { get; set; }

        [Required]
        public long Createdby { get; set; }
        public string? Createdbyname { get; set; }

        [Required]
        public long Modifiedby { get; set; }
        public string? Modifiedbyname { get; set; }

        [Required]
        public DateTime Datecreated { get; set; }

        [Required]
        public DateTime Datemodified { get; set; }
        public List<Systemblogparagraph>? Systemblogparagraph { get; set; }
    }
}
