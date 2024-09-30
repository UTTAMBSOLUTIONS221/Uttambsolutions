namespace DBL.Entities.Tokenization
{
    public class Tokenownership
    {
        public int Id { get; set; }
        public int Userid { get; set; }
        public int Softwaretokenid { get; set; }
        public int Tokenamount { get; set; } // Total tokens owned by the user
    }
}
