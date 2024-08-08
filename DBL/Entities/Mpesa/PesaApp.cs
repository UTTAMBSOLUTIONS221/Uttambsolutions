using DBL.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DBL.Entities.Mpesa
{
    [Table("PesaApps")]
    public class PesaApp : BaseEntity
    {
        [NotMapped]
        public static string TableName { get { return "PesaApps"; } }

        [Column("Id")]
        public int Id { get; set; }

        [Column("AppCode")]
        public int AppCode { get; set; }

        [Column("ServiceCode")]
        [Required()]
        public int ServiceCode { get; set; }

        [Column("AppName")]
        [Required()]
        [StringLength(30)]
        [Display(Name = "App Name")]
        public string AppName { get; set; }

        [Column("AppID")]
        [StringLength(20)]
        public string AppID { get; set; }

        [Column("AppToken")]
        [StringLength(250)]
        public string AppToken { get; set; }

        [Column("CreateDate")]
        public DateTime CreateDate { get; set; }

        [Column("LastUsed")]
        public DateTime LastUsed { get; set; }

        [Column("AppStatus")]
        public int AppStatus { get; set; }

        [Column("Attempts")]
        public int Attempts { get; set; }
    }
}
