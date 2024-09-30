namespace DBL.Entities.Tokenization
{
    public class Softwaretoken
    {
        public int Tokenid { get; set; }
        public string Tokenname { get; set; } // E.g., "SYS-TOKEN"
        public decimal Tokenprice { get; set; } // Price per token, e.g., $10
        public int Totalsupply { get; set; } // Total tokens for this software, e.g., 100,000
        public decimal Totalvalue => Tokenprice * Totalsupply; // Total value of the system
    }

}
