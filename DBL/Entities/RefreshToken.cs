namespace DBL.Entities
{
    public class RefreshToken
    {
        public int Refreshtokenid { get; set; }
        public string? Token { get; set; }
        public DateTime Expirydate { get; set; }
        public long Userid { get; set; }
    }
}
