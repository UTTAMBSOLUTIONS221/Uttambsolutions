using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DBL.Entities.Mpesa
{
    [Table("Payments")]
    public class Payment
    {
        [NotMapped]
        public static string TableName { get { return "Payments"; } }

        [Column("Id")]
        public int Id { get; set; }

        [Column("ServiceCode")]
        [Required()]
        public int ServiceCode { get; set; }

        [Column("PaymentRef")]
        [Required()]
        [StringLength(15)]
        public string PaymentRef { get; set; }

        [Column("AccountNo")]
        [Required()]
        [StringLength(20)]
        public string AccountNo { get; set; }

        [Column("AccountName")]
        [StringLength(35)]
        public string AccountName { get; set; }

        [Column("Amount")]
        [Required()]
        public decimal Amount { get; set; }

        [Column("PDate")]
        [Required()]
        public DateTime PDate { get; set; }

        [Column("PType")]
        [Required()]
        public int PType { get; set; }

        [Column("PStatus")]
        [Required()]
        public int PStatus { get; set; }

        [Column("TPRef")]
        [StringLength(15)]
        public string TPRef { get; set; }

        [Column("RespNo")]
        [StringLength(15)]
        public string RespNo { get; set; }

        [Column("RespMsg")]
        [StringLength(250)]
        public string RespMsg { get; set; }

        [Column("ExtRef")]
        [StringLength(15)]
        public string ExtRef { get; set; }

        [Column("Extra1")]
        [StringLength(100)]
        public string Extra1 { get; set; }

        [Column("Extra2")]
        [StringLength(100)]
        public string Extra2 { get; set; }

        [Column("Extra3")]
        [StringLength(100)]
        public string Extra3 { get; set; }

        [Column("Extra4")]
        [StringLength(100)]
        public string Extra4 { get; set; }

        [Column("TPStat")]
        public int TPStat { get; set; }

        [Column("TPMessage")]
        [StringLength(150)]
        public string TPMessage { get; set; }

        [Column("AppCode")]
        public int AppCode { get; set; }
    }
}
