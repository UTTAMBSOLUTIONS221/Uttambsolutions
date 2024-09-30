namespace DBL.Entities.Tokenization
{
    public class Tokentransfer
    {
        public int Id { get; set; }
        public int Fromuserid { get; set; } // The seller
        public int Touserid { get; set; } // The buyer
        public int Softwaretokenid { get; set; } // Token being transferred
        public int Tokenamount { get; set; } // Amount of tokens being transferred
        public decimal Totalcost { get; set; } // Price paid for the transfer
        public DateTime Transferdate { get; set; } // Transfer date
    }

}
