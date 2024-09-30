namespace DBL.Entities.Tokenization
{
    public class Tokenpurchase
    {
        public int Id { get; set; }
        public int Userid { get; set; } // Buyer (User)
        public int Softwaretokenid { get; set; } // Reference to SoftwareToken
        public int Tokenamount { get; set; } // Number of tokens purchased
        public decimal Totalcost { get; set; } // Total cost (TokenPrice * TokenAmount)
        public DateTime Purchasedate { get; set; } // Purchase timestamp
    }
}
