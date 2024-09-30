namespace DBL.Entities.Tokenization
{
    public class Tokenpurchase
    {
        public int Tokenpurchaseid { get; set; }
        public int Tokenid { get; set; } // Reference to SoftwareToken
        public int Userid { get; set; } // Buyer (User)
        public int Tokenamount { get; set; } // Number of tokens purchased
        public decimal Totalcost { get; set; } // Total cost (TokenPrice * TokenAmount)
        public DateTime Purchasedate { get; set; } // Purchase timestamp
    }
}
