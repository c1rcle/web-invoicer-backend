namespace WebInvoicer.Core.Token
{
    public class TokenConfiguration
    {
        public string JwtSecret { get; set; }

        public int TokenExpiryTime { get; set; }
    }
}
