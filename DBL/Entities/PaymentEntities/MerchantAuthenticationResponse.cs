namespace DBL.Entities.PaymentEntities
{
    public class MerchantAuthenticationResponse
    {
        public string? AccessToken { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime ExpiresIn { get; set; }
        public DateTime IssuedAt { get; set; }
        public string? TokenType { get; set; }
    }
}
