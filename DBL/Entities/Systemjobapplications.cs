using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DBL.Entities
{
    public class Systemjobapplications
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Jobapplicationid { get; set; }

        [Required]
        public long Userid { get; set; }

        [Required]
        public int Jobid { get; set; }
        public string? Title { get; set; }

        [Required]
        [StringLength(1000)]
        public string Coverletter { get; set; }

        public int Applicationstatus { get; set; } = 3;

        [Required]
        public DateTime Datecreated { get; set; } = DateTime.Now;
    }
}
