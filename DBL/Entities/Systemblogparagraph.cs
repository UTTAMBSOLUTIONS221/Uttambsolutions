using System.ComponentModel.DataAnnotations;

namespace DBL.Entities
{
    public class Systemblogparagraph
    {
        [Key]
        public long Blogparagraphid { get; set; }

        [Required]
        public long Blogid { get; set; }

        [StringLength(4000)]
        public string? Blogparagraphcontent { get; set; }

        [StringLength(200)]
        public string? Blogparagraphimageurl { get; set; }
        [Required]
        [StringLength(100)]
        public string? Blogparagraphimagename { get; set; }
        [Required]
        [StringLength(70)]
        public string? Blogparagraphimagesource { get; set; }
        [Required]
        public long Createdby { get; set; }

        [Required]
        public long Modifiedby { get; set; }

        [Required]
        public DateTime Datecreated { get; set; }

        [Required]
        public DateTime Datemodified { get; set; }
    }
}
